using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAppSettingsProvider settingsProvider;
        private readonly IMongoRepository<UserInfo> mongoRepository;

        public AuthController(IAppSettingsProvider settingsProvider, IMongoRepository<UserInfo> mongoRepository)
        {
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
        }

        [HttpPost("token")]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            JwtSecurityToken? token = null;

            await Task.Run(() =>
            {
                var user = mongoRepository.FindOne((a) => a.UserName.Equals(userInfo.UserName) && a.Password.Equals(userInfo.Password));

                //create claims details based on the user information
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, settingsProvider.Jwt?.Subject??""),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("DisplayName", user.DisplayName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email),
                        new Claim(ClaimTypes.Role,"Admin")
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settingsProvider.Jwt?.Key ?? ""));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                token = new JwtSecurityToken(
                  settingsProvider.Jwt?.Issuer,
                  settingsProvider.Jwt?.Audience,
                  claims,
                  expires: DateTime.UtcNow.AddMinutes(10),
                  signingCredentials: signIn);
            });


            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("testes")]
        public async Task<IActionResult> Post(string userInfo)
        {

            return Ok(userInfo);
        }

        [Authorize]
        [HttpPost("testesAuthorize")]
        public async Task<IActionResult> Authorize(string userInfo2)
        {

            return Ok(userInfo2);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("testesWitRole")]
        public async Task<IActionResult> AuthorizeWithRole(string userInfo)
        {

            return Ok(userInfo);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "NotAdmin")]
        [HttpPost("testesWitRoleNotAdmin")]
        public async Task<IActionResult> AuthorizeWithRoleNotAdmin(string userInfo)
        {

            return Ok(userInfo);
        }

    }
}
