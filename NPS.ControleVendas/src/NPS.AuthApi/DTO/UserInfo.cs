namespace NPS.AuthApi.Model
{
    [BsonCollection("users")]
    public class UserInfo : Document
    {
        public string DisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
