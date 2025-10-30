using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Domain;
using api.DTOs;
using api.BusinessLogic.Services;
using api.Utils;

namespace api.Controllers
{
    [ApiController]
    [Route("api/overtime")]
    public class ApprovalController : ControllerBase
    {
        private readonly OvertimeContext _context;
        private readonly IEmailService _emailService;

        public ApprovalController(OvertimeContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ✅ APPROVE REQUEST
        [HttpPost("approve/{overtimeId}")]
        public async Task<IActionResult> ApproveRequest(Guid overtimeId, [FromBody] ApprovalRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.ManagerEmail))
                return BadRequest(new { message = "Manager email required" });

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email.ToLower() == dto.ManagerEmail.ToLower());

            if (manager == null)
                return Unauthorized(new { message = "Manager not found" });

            var request = await _context.OvertimeRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OvertimeId == overtimeId && r.Status == OvertimeStatus.Pending);

            if (request == null)
                return NotFound(new { message = "Request not found or already processed" });

            if (request.User.ManagerId != manager.ManagerId)
                return Unauthorized(new { message = "Not authorized to approve this request" });

            // ✅ Update request and save approval
            request.Status = OvertimeStatus.Approved;
            request.UpdatedAt = DateTime.UtcNow;

            var approval = new Approval
            {
                ApprovalId = Guid.NewGuid(),
                OvertimeId = overtimeId,
                ManagerId = manager.ManagerId,
                ApprovedHours = request.TotalHours,
                Status = OvertimeStatus.Approved,
                Comments = dto.Comments
            };

            _context.OvertimeApprovals.Add(approval);
            await _context.SaveChangesAsync();

            // ✅ Generate Calendar link
            var calendarLink = CalendarLinkGenerator.GenerateGoogleCalendarLink(
                "Approved Overtime",
                request.Date,
                request.StartTime,
                request.EndTime,
                request.Justification
            );

            // ✅ Format times correctly
            var start = request.StartTime.ToString(@"hh\:mm");
            var end = request.EndTime.ToString(@"hh\:mm");

            // ✅ Build email
            var emailBody = $@"
<div style='font-family:Arial,sans-serif;'>
  <h2 style='color:#50B95D;'>Overtime Request Approved ✅</h2>
  <p>Your overtime request has been approved by <b>{manager.Email}</b>.</p>

  <div><b>Date:</b> {request.Date:dddd, MMM dd, yyyy}</div>
  <div><b>Schedule:</b> {start} - {end}</div>
  <div><b>Total Hours:</b> {request.TotalHours}</div>
  <div><b>Total Cost:</b> ₡{request.Cost:N2}</div>
  <div><b>Comments:</b> {dto.Comments ?? "No additional comments"}</div>
  <br/>
  <a href='{calendarLink}' target='_blank'
     style='background-color:#4285F4;
            color:white;
            padding:12px 24px;
            text-decoration:none;
            border-radius:6px;
            font-weight:bold;
            display:inline-block;'>
     🗓️ Add to Google Calendar
  </a>
</div>";

            await _emailService.SendOvertimeNotificationAsync(
                request.User.Email,
                "Overtime Approved ✅",
                emailBody
            );

            return Ok(new { message = "Request approved successfully" });
        }

        // ❌ REJECT REQUEST
        [HttpPost("reject/{overtimeId}")]
        public async Task<IActionResult> RejectRequest(Guid overtimeId, [FromBody] RejectRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.ManagerEmail))
                return BadRequest(new { message = "Manager email required" });

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email.ToLower() == dto.ManagerEmail.ToLower());

            if (manager == null)
                return Unauthorized(new { message = "Manager not found" });

            var request = await _context.OvertimeRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OvertimeId == overtimeId && r.Status == OvertimeStatus.Pending);

            if (request == null)
                return NotFound(new { message = "Request not found or already processed" });

            if (request.User.ManagerId != manager.ManagerId)
                return Unauthorized(new { message = "Not authorized to reject this request" });

            request.Status = OvertimeStatus.Rejected;
            request.UpdatedAt = DateTime.UtcNow;

            var rejection = new Approval
            {
                ApprovalId = Guid.NewGuid(),
                OvertimeId = overtimeId,
                ManagerId = manager.ManagerId,
                ApprovedHours = 0,
                Status = OvertimeStatus.Rejected,
                RejectionReason = dto.Reason,
                Comments = dto.Comments
            };

            _context.OvertimeApprovals.Add(rejection);
            await _context.SaveChangesAsync();

            var emailBody = $@"
<div style='font-family:Arial,sans-serif;'>
  <h2 style='color:#dc3545;'>Overtime Request Rejected ❌</h2>
  <p>Your overtime request was rejected by <b>{manager.Email}</b>.</p>

  <div><b>Date:</b> {request.Date:dddd, MMM dd, yyyy}</div>
  <div><b>Reason:</b> {dto.Reason}</div>
  <div><b>Comments:</b> {dto.Comments ?? "No additional comments"}</div>
</div>";

            await _emailService.SendOvertimeNotificationAsync(
                request.User.Email,
                "Overtime Request Rejected ❌",
                emailBody
            );

            return Ok(new { message = "Request rejected successfully" });
        }
    }
}
