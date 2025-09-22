namespace api.DTOs;
public class OvertimeCreateDto
{
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? CostCenter { get; set; }
    public string? Justification { get; set; }
}

public class OvertimeUpdateDto
{
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? CostCenter { get; set; }
    public string? Justification { get; set; }
}

public class OvertimeResponseDto
{
    public Guid OvertimeId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? CostCenter { get; set; }
    public string? Justification { get; set; }
    public string Status { get; set; } = default!; // "Pending" | "Approved" | "Rejected"
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal? Cost { get; set; }
}
