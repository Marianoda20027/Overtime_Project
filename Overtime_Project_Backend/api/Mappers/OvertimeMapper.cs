namespace api.Mappers;
using api.Domain;
using api.DTOs;

public static class OvertimeMapper
{
    public static OvertimeResponseDto ToDto(this OvertimeRequest e) => new()
    {
        OvertimeId = e.OvertimeId,
        UserId = e.UserId,
        Date = e.Date,
        StartTime = e.StartTime,
        EndTime = e.EndTime,
        CostCenter = e.CostCenter,
        Justification = e.Justification,
        Status = e.Status.ToString(),
        CreatedAt = e.CreatedAt,
        UpdatedAt = e.UpdatedAt,
        Cost = e.Cost
    };

    public static void ApplyUpdate(this OvertimeRequest e, OvertimeUpdateDto dto)
    {
        e.Date = dto.Date;
        e.StartTime = dto.StartTime;
        e.EndTime = dto.EndTime;
        e.CostCenter = dto.CostCenter;
        e.Justification = dto.Justification;
        e.UpdatedAt = DateTime.UtcNow;
    }
}
