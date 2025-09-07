using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private static readonly Dictionary<string, string> userSecrets = new(); // Guardamos secreto TOTP por usuario
        private static readonly HashSet<string> usersWith2FA = new(); // Usuarios con 2FA activado

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //valores test para retornar un ok en el login
            if (request.Username == "admin" && request.Password == "123")
            {
                if (usersWith2FA.Contains(request.Username))
                {
                    // Usuario tiene 2FA activado → solicita el código
                    return Ok(new { require2FA = true, message = "2FA required" });
                }

                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        // Endpoint para registrar 2FA (generar secreto)
        [HttpPost("register-2fa")]
        public IActionResult Register2FA([FromBody] Register2FARequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest(new { message = "Username required" });

            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);
            userSecrets[request.Username] = base32Secret;

            // Devuelve el secreto para que el frontend genere QR
            return Ok(new { secret = base32Secret });
        }

        // Endpoint para verificar 2FA
        [HttpPost("2fa")]
        public IActionResult Verify2FA([FromBody] Verify2FARequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Token))
                return BadRequest(new { message = "Username and token required" });

            if (!userSecrets.ContainsKey(request.Username))
                return BadRequest(new { message = "2FA not registered for user" });

            var totp = new Totp(Base32Encoding.ToBytes(userSecrets[request.Username]));
            bool isValid = totp.VerifyTotp(request.Token, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (!isValid)
                return Unauthorized(new { message = "Invalid 2FA code" });

            // Activar 2FA si aún no está activado
            usersWith2FA.Add(request.Username);

            // Genera JWT al validar correctamente
            var token = GenerateJwtToken(request.Username);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

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
