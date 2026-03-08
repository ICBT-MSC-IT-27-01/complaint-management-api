namespace Cd.Cms.Application.DTOs.Permissions
{
    public sealed class RolePermissionDto
    {
        public string Module { get; set; } = string.Empty;
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
    }
}
