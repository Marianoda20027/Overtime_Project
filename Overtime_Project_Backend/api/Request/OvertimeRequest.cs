namespace api.Request
{
    public class OvertimeRequest
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string Date { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty; // string en vez de TimeSpan
        public string EndTime { get; set; } = string.Empty;   // string en vez de TimeSpan
        public string Justification { get; set; } = string.Empty;
        public string? CostCenter { get; set; }
    }
}