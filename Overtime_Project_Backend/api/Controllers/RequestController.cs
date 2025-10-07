using api.DTOs;
using api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Controllers
{
    [Route("api/overtime")]
    [ApiController]
    public class RequestController  : ControllerBase
    {
        private readonly OvertimeContext _context;

        public RequestController (OvertimeContext context)
        {
            _context = context;
        }

      [HttpPost("create")]
public async Task<IActionResult> CreateOvertimeRequest([FromBody] OvertimeCreateDto overtimeCreateDto)
{
    var userEmail = overtimeCreateDto.Email; 

    if (string.IsNullOrEmpty(userEmail))
    {
        return Unauthorized(new { message = "Email is required." });
    }

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
    if (user == null)
    {
        return Unauthorized(new { message = "User not found." });
    }

    // Usamos directamente TotalHours enviado desde el frontend
    var cost = (int)(overtimeCreateDto.TotalHours * user.Salary);  // Convertido a int aquí

    var overtimeRequest = new OvertimeRequest
    {
        UserId = user.UserId,
        Date = overtimeCreateDto.Date,
        StartTime = overtimeCreateDto.StartTime,
        EndTime = overtimeCreateDto.EndTime,
        Justification = overtimeCreateDto.Justification,
        Status = OvertimeStatus.Pending,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        TotalHours = overtimeCreateDto.TotalHours,
        Cost = cost
    };

    _context.OvertimeRequests.Add(overtimeRequest);
    await _context.SaveChangesAsync();

    var responseDto = new OvertimeResponseDto
    {
        OvertimeId = overtimeRequest.OvertimeId,
        UserId = overtimeRequest.UserId,
        Date = overtimeRequest.Date,
        StartTime = overtimeRequest.StartTime,
        EndTime = overtimeRequest.EndTime,
        Justification = overtimeRequest.Justification,
        Status = overtimeRequest.Status.ToString(),
        CreatedAt = overtimeRequest.CreatedAt,
        UpdatedAt = overtimeRequest.UpdatedAt,
        TotalHours = overtimeRequest.TotalHours,
        Cost = overtimeRequest.Cost
    };

    return Ok(responseDto);
}


}


}
