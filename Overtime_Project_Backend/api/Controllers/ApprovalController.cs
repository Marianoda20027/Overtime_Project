using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Domain;
using api.DTOs;
using api.BusinessLogic.Services;

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

        // ✅ APROBAR SOLICITUD
        [HttpPost("approve/{overtimeId}")]
        public async Task<IActionResult> ApproveRequest(Guid overtimeId, [FromBody] ApprovalRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.ManagerEmail))
                return BadRequest(new { message = "Email del manager requerido" });

            // 🔹 Buscar manager por email
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email.ToLower() == dto.ManagerEmail.ToLower());

            if (manager == null)
                return Unauthorized(new { message = "Manager no encontrado" });

            // 🔹 Buscar la solicitud
            var request = await _context.OvertimeRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OvertimeId == overtimeId && r.Status == OvertimeStatus.Pending);

            if (request == null)
                return NotFound(new { message = "Solicitud no encontrada o ya procesada" });

            // 🔹 Verificar que el manager esté autorizado
            if (request.User.ManagerId != manager.ManagerId)
                return Unauthorized(new { message = "No autorizado para aprobar esta solicitud" });

            // ✅ Aprobar
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

            // 📧 Enviar email al usuario
            await _emailService.SendOvertimeNotificationAsync(
                request.User.Email,
                "Solicitud de Horas Extra Aprobada",
                $"<div class='info-row'><div class='info-label'>Fecha:</div><div class='info-value'>{request.Date:dddd, dd/MM/yyyy}</div></div>" +
                $"<div class='info-row'><div class='info-label'>Horario:</div><div class='info-value'>{request.StartTime:hh\\:mm} - {request.EndTime:hh\\:mm}</div></div>" +
                $"<div class='info-row'><div class='info-label'>Total de horas:</div><div class='info-value'>{request.TotalHours} horas</div></div>" +
                $"<div class='info-row'><div class='info-label'>Costo total:</div><div class='info-value'>₡{request.Cost:N2}</div></div>" +
                $"<div class='info-row'><div class='info-label'>Comentarios:</div><div class='info-value'>{dto.Comments ?? "Sin comentarios adicionales"}</div></div>"
            );

            return Ok(new { message = "Solicitud aprobada correctamente" });
        }

        // ❌ RECHAZAR SOLICITUD
        [HttpPost("reject/{overtimeId}")]
        public async Task<IActionResult> RejectRequest(Guid overtimeId, [FromBody] RejectRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.ManagerEmail))
                return BadRequest(new { message = "Email del manager requerido" });

            // 🔹 Buscar manager por email
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Email.ToLower() == dto.ManagerEmail.ToLower());

            if (manager == null)
                return Unauthorized(new { message = "Manager no encontrado" });

            // 🔹 Buscar la solicitud
            var request = await _context.OvertimeRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.OvertimeId == overtimeId && r.Status == OvertimeStatus.Pending);

            if (request == null)
                return NotFound(new { message = "Solicitud no encontrada o ya procesada" });

            // 🔹 Verificar que el manager esté autorizado
            if (request.User.ManagerId != manager.ManagerId)
                return Unauthorized(new { message = "No autorizado para rechazar esta solicitud" });

            // ❌ Rechazar
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

            // 📧 Enviar email al usuario
            await _emailService.SendOvertimeNotificationAsync(
                request.User.Email,
                "Solicitud de Horas Extra RECHAZADA",
                $"<strong> Fecha:</strong> {request.Date:dd/MM/yyyy}<br>" +
                $"<strong> Horario:</strong> {request.StartTime:hh\\:mm} - {request.EndTime:hh\\:mm}<br>" +
                $"<strong> Total horas:</strong> {request.TotalHours}<br><br>" +
                $"<strong> Motivo:</strong> {dto.Reason}<br>" +
                $"<strong> Comentarios:</strong> {dto.Comments ?? "N/A"}"
            );

            return Ok(new { message = "Solicitud rechazada correctamente" });
        }
    }
}