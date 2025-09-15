using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.Request;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace api.Controllers
{
    [ApiController]
    [Route("api/overtime")]
    public class OvertimeRequestsController : ControllerBase
    {
        private static readonly List<OvertimeRequest> _requests = new();
        private static int _nextId = 1;

        [HttpPost("request")]
        [AllowAnonymous]
        public IActionResult Create([FromBody] OvertimeRequest model)
        {
            if (model == null)
                return BadRequest(new { message = "Datos inválidos" });

            model.Id = _nextId++;
            model.Username = User.Identity?.Name ?? "unknown";
            _requests.Add(model);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);

            if (request == null)
                return NotFound(new { message = "Solicitud no encontrada" });

            return Ok(request);
        }

        // Listar todas las solicitudes → permitido a Manager y People_Ops
        [HttpGet("all")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_requests);
        }
    }
}