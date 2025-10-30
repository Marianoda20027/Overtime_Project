using System;

namespace api.Domain
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "sent";

        public Guid? OvertimeId { get; set; } // Relación con la hora extra
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        public User User { get; set; }
    }
}
