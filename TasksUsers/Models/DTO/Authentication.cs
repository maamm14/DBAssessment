namespace UsersTasks.Models.DTO
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
