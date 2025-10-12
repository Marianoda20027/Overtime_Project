namespace api.Domain
{
    public class Approval
    {
        public Guid ApprovalId { get; set; }
        public Guid OvertimeId { get; set; }
        public int ManagerId { get; set; }
        public decimal ApprovedHours { get; set; }
        public DateTime ApprovalDate { get; set; } = DateTime.UtcNow;
        public OvertimeStatus Status { get; set; }
        public string? Comments { get; set; }
        public string? RejectionReason { get; set; }

        // Navigation properties
        public OvertimeRequest? Overtime { get; set; }
        public Manager? Manager { get; set; }
    }
}