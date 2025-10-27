namespace api.Domain
{
    public class HumanResource
    {
        public int HumanResourceId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}