using System;

namespace api.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public decimal Salary { get; set; }

        // 🔹 FK hacia Manager
        public int? ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public ICollection<OvertimeRequest>? OvertimeRequests { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
