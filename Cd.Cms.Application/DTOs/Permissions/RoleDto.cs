namespace Cd.Cms.Application.DTOs.Permissions
{
    public sealed class RoleDto
    {
        public long Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; }
    }
}
