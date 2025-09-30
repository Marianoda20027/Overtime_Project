  using System.Text.Json.Serialization;
namespace api.Request
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }




    public class OTPRequest
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("OTP")]
        public string OTP { get; set; } = null!;
    }
}

