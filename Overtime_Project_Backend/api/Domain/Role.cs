namespace api.Domain;

public class Role
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = default!;
    public string? Permissions { get; set; }
}
