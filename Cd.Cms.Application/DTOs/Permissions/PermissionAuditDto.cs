namespace Cd.Cms.Application.DTOs.Permissions
{
    public sealed class PermissionAuditDto
    {
        public long Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime ChangedDateTime { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}
