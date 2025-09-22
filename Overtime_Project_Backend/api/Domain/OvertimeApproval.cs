namespace api.Domain;

public class Approval
{
    public Guid ApprovalId { get; set; }
    public Guid OvertimeId { get; set; }
    public Guid ManagerId { get; set; }
    public decimal ApprovedHours { get; set; }
    public DateTime ApprovalDate { get; set; } = DateTime.UtcNow;
    public OvertimeStatus Status { get; set; }  // Approved / Rejected
    public string? Comments { get; set; }
    public string? RejectionReason { get; set; }
    public OvertimeRequest? Overtime { get; set; }
    public User? Manager { get; set; }
}