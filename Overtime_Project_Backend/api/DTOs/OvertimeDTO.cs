namespace api.DTOs;
  public class OvertimeCreateDto
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Justification { get; set; }
        public decimal TotalHours { get; set; }  
        public string Email { get; set; }
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
        public string Justification { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalHours { get; set; }  // Añadí TotalHours
        public decimal Cost { get; set; }
    }
