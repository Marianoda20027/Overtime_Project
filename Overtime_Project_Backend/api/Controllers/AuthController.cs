using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("auth")] //ruta base a llamar desde el login
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")] // 👈 ruta final: /auth/login
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 🔐 Aquí deberías validar contra tu base de datos
            if (request.Username == "admin" && request.Password == "1234")
            {
                var token = GenerateToken(request.Username, "Admin");
                return Ok(new { token });
            }

            if (request.Username == "user" && request.Password == "1234")
            {
                var token = GenerateToken(request.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized("Usuario o contraseña inválidos");
        }

        private string GenerateToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere")
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: null,    
                audience: null,  
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10), //tiempo de expiracion del token
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTO para login
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}