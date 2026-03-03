namespace Cd.Cms.Application.DTOs.Users
{
    public sealed class AuthUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
