using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NPS.AuthApi.Data;
using NPS.AuthApi.Domain;
using NPS.AuthApi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NPS.AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/token")]
    public class AuthController : Controller
    {
        private readonly IAppSettingsProvider settingsProvider;
        private readonly IMongoRepository<UserInfo> mongoRepository;

        public AuthController(IAppSettingsProvider settingsProvider, IMongoRepository<UserInfo> mongoRepository)
        {
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            var user = mongoRepository.FindOne((a) => a.UserName.Equals(userInfo.UserName) && a.Password.Equals(userInfo.Password));

            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, settingsProvider.Jwt.Subject??""),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("DisplayName", user.DisplayName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settingsProvider.Jwt.Key ?? ""));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                settingsProvider.Jwt.Issuer,
                settingsProvider.Jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
