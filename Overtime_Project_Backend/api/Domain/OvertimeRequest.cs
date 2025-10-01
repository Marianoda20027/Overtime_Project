namespace api.Domain
{
    public class OvertimeRequest
    {
        public Guid OvertimeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }  // Cambié de string a TimeSpan
        public TimeSpan EndTime { get; set; }    // Cambié de string a TimeSpan
        public string Justification { get; set; }
        public OvertimeStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalHours { get; set; }  // TotalHours es decimal
        public decimal Cost { get; set; }

        // Relación con el usuario
        public User User { get; set; }

        // Relación con las aprobaciones
        public ICollection<Approval> Approvals { get; set; } = new List<Approval>();
    }
      public enum OvertimeStatus
    {
        Pending,    // Solicitud pendiente
        Approved,   // Solicitud aprobada
        Rejected    // Solicitud rechazada
    }
}
