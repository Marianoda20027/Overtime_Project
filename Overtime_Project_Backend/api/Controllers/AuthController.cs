using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using api.Request;
using api.BusinessLogic.Services;
using api.JWTToken;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly OvertimeContext _context;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, OvertimeContext context, IConfiguration config)
        {
            _authService = authService;
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Email and password are required." });

            var result = await _authService.AuthenticateUserAsync(request.Username, request.Password);
            if (!result.success)
                return Unauthorized(new { message = result.message });

            return Ok(new { message = result.message });
        }

        [HttpPost("2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] OTPRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.OTP))
                return BadRequest(new { message = "Username and OTP are required." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Username.ToLower());
            if (user == null)
                return Unauthorized(new { message = "User not found." });

            if (!OTPStore.ValidateOTP(user.Email, request.OTP))
                return Unauthorized(new { message = "Invalid or expired OTP." });

            // Generar JWT
            var tokenCreator = new TokenCreator(_config);
            var jwt = tokenCreator.CreateToken(user.Email);

            return Ok(new { message = "2FA successful.", token = jwt });
        }
    }
}
