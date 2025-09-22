using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.Domain;
using api.DTOs;
using api.Mappers;

namespace api.Controllers;

[ApiController]
[Route("api/overtime")]
public class OvertimeRequestsController : ControllerBase
{
    // Simulación in-memory (cambiá por DbContext luego)
    internal static readonly List<OvertimeRequest> _requests = new();

    [HttpPost("request")]
    [Authorize] // antes era AllowAnonymous
    public IActionResult Create([FromBody] OvertimeCreateDto dto)
    {
        if (dto is null) return BadRequest(new { message = "Datos inválidos" });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var entity = new OvertimeRequest
        {
            OvertimeId = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            CostCenter = dto.CostCenter,
            Justification = dto.Justification,
            Status = OvertimeStatus.Pending
        };
        _requests.Add(entity);

        return CreatedAtAction(nameof(GetById), new { id = entity.OvertimeId }, entity.ToDto());
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public IActionResult GetById(Guid id)
    {
        var e = _requests.FirstOrDefault(r => r.OvertimeId == id);
        return e is null ? NotFound(new { message = "Solicitud no encontrada" }) : Ok(e.ToDto());
    }

    [HttpGet("all")]
    [Authorize(Roles = "Manager,People_Ops")]
    public IActionResult GetAll() => Ok(_requests.Select(x => x.ToDto()));

    [HttpGet("user/{userId:guid}")]
    [Authorize]
    public IActionResult GetByUser(Guid userId)
        => Ok(_requests.Where(r => r.UserId == userId).Select(r => r.ToDto()));

    [HttpPut("{id:guid}")]
    [Authorize]
    public IActionResult Update(Guid id, [FromBody] OvertimeUpdateDto dto)
    {
        var e = _requests.FirstOrDefault(r => r.OvertimeId == id);
        if (e is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (e.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "No se puede modificar una solicitud ya procesada" });

        e.ApplyUpdate(dto);
        return Ok(e.ToDto());
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public IActionResult Delete(Guid id)
    {
        var e = _requests.FirstOrDefault(r => r.OvertimeId == id);
        if (e is null) return NotFound(new { message = "Solicitud no encontrada" });
        if (e.Status != OvertimeStatus.Pending)
            return BadRequest(new { message = "No se puede eliminar una solicitud ya procesada" });

        _requests.Remove(e);
        return Ok(new { message = "Solicitud eliminada exitosamente" });
    }
}
