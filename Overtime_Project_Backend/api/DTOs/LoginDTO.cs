namespace api.Request
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class Register2FARequest
    {
        public string? Username { get; set; }
    }

    public class Verify2FARequest
    {
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}