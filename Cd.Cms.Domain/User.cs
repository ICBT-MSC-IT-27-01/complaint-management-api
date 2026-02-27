namespace Cd.Cms.Domain
{
    public class User : BaseDomainEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = "Agent";
        public bool IsLocked { get; set; } = false;
        public DateTime? LastLoginDateTime { get; set; }
    }
}
