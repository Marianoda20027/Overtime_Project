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
                return BadRequest(new { message = "Invalid data" });

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
                return NotFound(new { message = "Request not found" });

            return Ok(request);
        }

        // List all requests → Allowed for Manager and People_Ops
        [HttpGet("all")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_requests);
        }

        // Accept a specific overtime request
        [HttpPost("{id:int}/accept")]
        [Authorize(Roles = "Manager, People_Ops")]
        public IActionResult AcceptRequest(int id, [FromBody] ApprovalRequest approval)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound(new { message = "Request not found" });

            // Add comments and calculate cost
            request.Status = "Approved";
            request.Comments = approval.Comments;
            request.Cost = approval.Cost; // Assume cost is passed in the request

            return Ok(new { message = "Request accepted", request });
        }

        // Reject a specific overtime request
        [HttpPost("{id:int}/reject")]
        [Authorize(Roles = "Manager, People_Ops")]
        public IActionResult RejectRequest(int id, [FromBody] RejectRequest reject)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
                return NotFound(new { message = "Request not found" });

            // Add rejection reason and comments
            request.Status = "Rejected";
            request.RejectionReason = reject.Reason;
            request.Comments = reject.Comments;

            return Ok(new { message = "Request rejected", request });
        }
    }
}
