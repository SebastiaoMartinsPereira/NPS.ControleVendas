using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NPS.AuthApi.ViewModel
{
    public class UserLogin
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginResponse
    {
        public UserLoginResponse(string displayName, string userName, string email, string token, Claim[] claims)
        {
            DisplayName = displayName;
            UserName = userName;
            Email = email;
            Token = token;
            Claims = claims;
        }

        public string DisplayName { get; }
        public string UserName { get; }
        public string Email { get; } 
        public string Token { get; }
        public Claim[] Claims { get; }
    }
}
