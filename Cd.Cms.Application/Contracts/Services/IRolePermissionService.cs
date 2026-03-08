using Cd.Cms.Application.DTOs.Permissions;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IRolePermissionService
    {
        Task<List<RoleDto>> GetRolesAsync();
        Task<List<RolePermissionDto>> GetByRoleAsync(string role);
        Task SaveAsync(SaveRolePermissionsRequest request, long actorUserId);
        Task DuplicateRoleAsync(string role, string newRole, long actorUserId);
        Task<List<PermissionAuditDto>> GetAuditTrailAsync(string? role, int top);
    }
}
