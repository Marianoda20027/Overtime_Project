namespace api.Request
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

   public class OTPRequest
    {
        public string Username { get; set; } = null!;
        public string OTP { get; set; } = null!;
    }
}