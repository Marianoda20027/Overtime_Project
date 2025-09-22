using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.Domain;
using api.DTOs;
using api.Mappers;
using api.Data;


namespace api.Controllers;

[ApiController]
[Route("api/overtime/approval")]
public class OvertimeApprovalController : ControllerBase
{
    private readonly OvertimeContext _db;

    public OvertimeApprovalController(OvertimeContext db)
    {
        _db = db;
    }

    [HttpPost("{id:guid}/accept")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult AcceptRequest(Guid id, [FromBody] ApprovalRequestDto body)
    {
        var req = _db.OvertimeRequests.FirstOrDefault(r => r.OvertimeId == id);
        if (req is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (req.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "La solicitud ya fue procesada" });

        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(managerId)) return Unauthorized();

        // actualizar request
        req.Status = OvertimeStatus.Approved;
        req.Cost = CalcularCosto(req, body.ApprovedHours);
        req.UpdatedAt = DateTime.UtcNow;

        // crear approval
        var approval = new Approval
        {
            ApprovalId = Guid.NewGuid(),
            OvertimeId = req.OvertimeId,
            ManagerId = Guid.Parse(managerId),
            ApprovedHours = body.ApprovedHours,
            Status = OvertimeStatus.Approved,
            Comments = body.Comments,
            ApprovalDate = DateTime.UtcNow
        };

        _db.OvertimeApprovals.Add(approval);
        _db.SaveChanges();

        return Ok(new {
            message = "Solicitud aprobada",
            request = req.ToDto(),
            approval = approval.ToDto()
        });
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult RejectRequest(Guid id, [FromBody] RejectRequestDto body)
    {
        var req = _db.OvertimeRequests.FirstOrDefault(r => r.OvertimeId == id);
        if (req is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (req.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "La solicitud ya fue procesada" });

        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(managerId)) return Unauthorized();

        // actualizar request
        req.Status = OvertimeStatus.Rejected;
        req.UpdatedAt = DateTime.UtcNow;

        // crear approval
        var approval = new Approval
        {
            ApprovalId = Guid.NewGuid(),
            OvertimeId = req.OvertimeId,
            ManagerId = Guid.Parse(managerId),
            ApprovedHours = 0,
            Status = OvertimeStatus.Rejected,
            Comments = body.Comments,
            RejectionReason = body.Reason,
            ApprovalDate = DateTime.UtcNow
        };

        _db.OvertimeApprovals.Add(approval);
        _db.SaveChanges();

        return Ok(new {
            message = "Solicitud rechazada",
            request = req.ToDto(),
            approval = approval.ToDto()
        });
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetPendingRequests()
    {
        var pending = _db.OvertimeRequests
            .Where(r => r.Status == OvertimeStatus.Pending)
            .Select(r => r.ToDto()) // mapper a DTO
            .ToList();

        return Ok(pending);
    }

    private static decimal CalcularCosto(OvertimeRequest r, decimal approvedHours)
    {
        // ejemplo básico: salario por hora (asumiendo 160 horas al mes)
        var hourlyRate = r.User?.Salary / 160 ?? 10m;
        return Math.Round(approvedHours * hourlyRate, 2);
    }
}
