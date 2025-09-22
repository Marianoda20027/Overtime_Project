namespace api.DTOs
{

public class ApprovalRequestDto   // payload de aprobación
{
    public decimal ApprovedHours { get; set; }
    public string? Comments { get; set; }
}

public class RejectRequestDto     // payload de rechazo
{
    public string Reason { get; set; } = default!;
    public string? Comments { get; set; }
}

public class ApprovalResponseDto
{
    public Guid ApprovalId { get; set; }
    public Guid OvertimeId { get; set; }
    public Guid ManagerId { get; set; }
    public decimal ApprovedHours { get; set; }
    public DateTime ApprovalDate { get; set; }
    public string Status { get; set; } = default!;
    public string? Comments { get; set; }
    public string? RejectionReason { get; set; }
}

}