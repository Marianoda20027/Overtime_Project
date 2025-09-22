namespace api.Domain;


public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = default!;
    public DateTime DateSent { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "sent";
    public User? User { get; set; }
}