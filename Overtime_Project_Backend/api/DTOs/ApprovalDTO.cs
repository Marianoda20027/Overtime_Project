namespace api.DTOs
{
    public class ApprovalRequestDto
    {
        public string ManagerEmail { get; set; } = default!;
        public string? Comments { get; set; }
    }

    public class RejectRequestDto
    {
        public string ManagerEmail { get; set; } = default!;
        public string Reason { get; set; } = default!;
        public string? Comments { get; set; }
    }

    public class ApprovalResponseDto
    {
        public Guid ApprovalId { get; set; }
        public Guid OvertimeId { get; set; }
        public int ManagerId { get; set; }
        public decimal ApprovedHours { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Status { get; set; } = default!;
        public string? Comments { get; set; }
        public string? RejectionReason { get; set; }
    }
}
