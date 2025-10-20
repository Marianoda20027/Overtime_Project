namespace api.BusinessLogic.Services.Reports
{
    public class ReportData
    {
        public int TotalRequests { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public double AvgResponseTime { get; set; }
        public decimal TotalCost { get; set; }
        public List<TopUser> TopUsers { get; set; } = new();
    }

    public class TopUser
    {
        public string UserName { get; set; } = "";
        public decimal TotalHours { get; set; }
    }
}
