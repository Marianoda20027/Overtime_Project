namespace api.Request
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

  public class VerifyOTPRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}