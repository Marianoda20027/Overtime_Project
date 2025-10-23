using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly OvertimeContext _context;

        public ManagerController(OvertimeContext context)
        {
            _context = context;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByManagerEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email del manager requerido" });

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());

            if (manager == null)
                return NotFound(new { message = "Manager no encontrado" });

            var userIds = await _context.Users
                .Where(u => u.ManagerId == manager.ManagerId)
                .Select(u => u.UserId)
                .ToListAsync();

            if (!userIds.Any())
                return Ok(new List<object>()); 
                
            var requests = await _context.OvertimeRequests
                .Include(r => r.User)
                .Where(r => userIds.Contains(r.UserId))
                .Select(r => new
                {
                    id = r.OvertimeId,
                    person = r.User.Email,
                    date = r.Date,
                    startTime = r.StartTime,
                    endTime = r.EndTime,
                    justification = r.Justification,
                    status = r.Status.ToString(),
                    cost = r.Cost,
                    totalHours = r.TotalHours,
                    salary = r.User.Salary
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _context.OvertimeRequests
                .Include(r => r.User)
                .Select(r => new
                {
                    id = r.OvertimeId,
                    person = r.User.Email,
                    date = r.Date,
                    startTime = r.StartTime,
                    endTime = r.EndTime,
                    justification = r.Justification,
                    status = r.Status.ToString(),
                    cost = r.Cost,
                    totalHours = r.TotalHours,
                    salary = r.User.Salary
                })
                .ToListAsync();

            return Ok(requests);
        }
    }
}
