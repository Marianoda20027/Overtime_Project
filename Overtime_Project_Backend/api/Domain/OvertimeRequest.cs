namespace api.Domain;

public class OvertimeRequest
{
    public Guid OvertimeId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? CostCenter { get; set; }
    public string? Justification { get; set; }
    public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public decimal? Cost { get; set; }
    public User? User { get; set; }
    public ICollection<Approval>? Approvals { get; set; }
}
