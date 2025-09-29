using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using api.Request;
using api.BusinessLogic.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Endpoint de login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var result = await _authService.AuthenticateUserAsync(request.Username, request.Password);

            if (!result.success)
                return Unauthorized(new { message = result.message });

            return Ok(new { message = result.message });
        }
    }
}
