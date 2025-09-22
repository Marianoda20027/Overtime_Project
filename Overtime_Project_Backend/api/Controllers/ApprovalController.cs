using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.Domain;
using api.DTOs;
using api.Mappers;


namespace api.Controllers;

[ApiController]
[Route("api/overtime/approval")]
public class OvertimeApprovalController : ControllerBase
{
    private static readonly List<Approval> _approvals = new();

    [HttpPost("{id:guid}/accept")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult AcceptRequest(Guid id, [FromBody] ApprovalRequestDto body)
    {
        var req = OvertimeRequestsController._requests.FirstOrDefault(r => r.OvertimeId == id);
        if (req is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (req.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "La solicitud ya ha sido procesada" });

        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(managerId)) return Unauthorized();

        req.Status = OvertimeStatus.Approved;
        req.Cost = CalcularCosto(req, body.ApprovedHours);

        var approval = new Approval
        {
            ApprovalId = Guid.NewGuid(),
            OvertimeId = req.OvertimeId,
            ManagerId = Guid.Parse(managerId),
            ApprovedHours = body.ApprovedHours,
            Status = OvertimeStatus.Approved,
            Comments = body.Comments
        };
        _approvals.Add(approval);

        return Ok(new
        {
            message = "Solicitud aprobada",
            request = req.ToDto(),
            approval = new ApprovalResponseDto
            {
                ApprovalId = approval.ApprovalId,
                OvertimeId = approval.OvertimeId,
                ManagerId = approval.ManagerId,
                ApprovedHours = approval.ApprovedHours,
                ApprovalDate = approval.ApprovalDate,
                Status = approval.Status.ToString(),
                Comments = approval.Comments
            }
        });
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult RejectRequest(Guid id, [FromBody] RejectRequestDto body)
    {
        var req = OvertimeRequestsController._requests.FirstOrDefault(r => r.OvertimeId == id);
        if (req is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (req.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "La solicitud ya ha sido procesada" });

        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(managerId)) return Unauthorized();

        req.Status = OvertimeStatus.Rejected;

        var approval = new Approval
        {
            ApprovalId = Guid.NewGuid(),
            OvertimeId = req.OvertimeId,
            ManagerId = Guid.Parse(managerId),
            ApprovedHours = 0,
            Status = OvertimeStatus.Rejected,
            Comments = body.Comments,
            RejectionReason = body.Reason
        };
        _approvals.Add(approval);

        return Ok(new
        {
            message = "Solicitud rechazada",
            request = req.ToDto(),
            approval = new ApprovalResponseDto
            {
                ApprovalId = approval.ApprovalId,
                OvertimeId = approval.OvertimeId,
                ManagerId = approval.ManagerId,
                ApprovedHours = 0,
                ApprovalDate = approval.ApprovalDate,
                Status = approval.Status.ToString(),
                Comments = approval.Comments
            }
        });
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetPending()
        => Ok(OvertimeRequestsController._requests
            .Where(r => r.Status == OvertimeStatus.Pending)
            .Select(r => r.ToDto()));

    [HttpGet("approved")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetApproved()
        => Ok(OvertimeRequestsController._requests
            .Where(r => r.Status == OvertimeStatus.Approved)
            .Select(r => r.ToDto()));

    [HttpGet("rejected")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetRejected()
        => Ok(OvertimeRequestsController._requests
            .Where(r => r.Status == OvertimeStatus.Rejected)
            .Select(r => r.ToDto()));

    [HttpGet("history/{managerId:guid}")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetApprovalHistory(Guid managerId)
        => Ok(_approvals.Where(a => a.ManagerId == managerId)
                        .Select(a => new ApprovalResponseDto {
                            ApprovalId = a.ApprovalId,
                            OvertimeId = a.OvertimeId,
                            ManagerId = a.ManagerId,
                            ApprovedHours = a.ApprovedHours,
                            ApprovalDate = a.ApprovalDate,
                            Status = a.Status.ToString(),
                            Comments = a.Comments,
                        }));

    private static decimal CalcularCosto(OvertimeRequest r, decimal approvedHours)
    {
        // Ejemplo simple: si no hay salario del usuario en memoria, usar tarifa fija
        const decimal tarifa = 10m; 
        return Math.Round(approvedHours * tarifa, 2);
    }
}
