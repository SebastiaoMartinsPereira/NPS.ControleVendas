using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using NPS.AuthApi.Data;
using NPS.AuthApi.Domain;
using NPS.AuthApi.Model;
using NPS.AuthApi.ViewModel;
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
        private readonly IStringLocalizer<Resources.Resources> _localizer;


        public AuthController(
            IAppSettingsProvider settingsProvider,
            IMongoRepository<UserInfo> mongoRepository,
            IStringLocalizer<Resources.Resources> localizer)
        {
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
            _localizer = localizer;
        }

        [HttpPost("token", Name = "Token")]
        public async Task<IActionResult> Post(UserLogin userInfo)
        {
            JwtSecurityToken? token = null;
            Claim[]? claims = null;

            await Task.Run(() =>
            {
                UserInfo? user = mongoRepository.FindOne((a) => (a.UserName.Equals(userInfo.Login) || a.Email.Equals(userInfo.Login)) && a.Password.Equals(userInfo.Password));

                if (user is null)
                {
                    ModelState.AddModelError(_localizer["InvalidLoginPass"], _localizer["InvalidLoginPass"]);
                    return;
                }

                //create claims details based on the user information
                claims = new[] {
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


            if (ModelState.ErrorCount > 0) return ValidationProblem(ModelState);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new UserLoginResponse(claims!.Where(c => c.Type.Equals("DisplayName")).First().Value, claims!.Where(c => c.Type.Equals("UserName")).First().Value, claims!.Where(c => c.Type.Equals("Email")).First().Value, tokenString, claims));
        }

        [HttpPost("testes", Name = "Testes")]
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
        [HttpPost("testesWitRole", Name = "TestesRole")]
        public async Task<IActionResult> AuthorizeWithRole(string userInfo)
        {

            return Ok(userInfo);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "NotAdmin")]
        [HttpPost("testesWitRoleNotAdmin", Name = "TestesWitAdminRoles")]
        public async Task<IActionResult> AuthorizeWithRoleNotAdmin(string userInfo)
        {

            return Ok(userInfo);
        }

    }
}
