using OtpNet;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Serialization;
using api.Request;
using api.JWTToken;

namespace api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        private static readonly Dictionary<string, string> userSecrets = new();
        private static readonly HashSet<string> usersWith2FA = new();

        private readonly TokenCreator tokenCreator;

        public AuthController(IConfiguration config)
        {
            tokenCreator = new TokenCreator(config);
        }

        [HttpPost("login")]
        [AllowAnonymous]
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

                var token = tokenCreator.CreateToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        // Endpoint para registrar 2FA (generar secreto)
        [HttpPost("register-2fa")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
            var token = tokenCreator.CreateToken(request.Username);
            return Ok(new { token });
        }


    }
}
