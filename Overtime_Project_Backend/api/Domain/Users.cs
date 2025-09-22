namespace api.Domain;

public enum OvertimeStatus { Pending, Approved, Rejected }

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public decimal Salary { get; set; }
    public ICollection<OvertimeRequest>? OvertimeRequests { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}

