using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Permissions;

namespace Cd.Cms.Application.Services
{
    public sealed class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _repo;
        public RolePermissionService(IRolePermissionRepository repo) => _repo = repo;

        public Task<List<RoleDto>> GetRolesAsync() => _repo.GetRolesAsync();

        public Task<List<RolePermissionDto>> GetByRoleAsync(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role is required.");
            return _repo.GetByRoleAsync(role.Trim());
        }

        public Task SaveAsync(SaveRolePermissionsRequest request, long actorUserId)
        {
            if (request == null) throw new ArgumentException("Request body is required.");
            if (string.IsNullOrWhiteSpace(request.Role)) throw new ArgumentException("Role is required.");
            if (request.Permissions == null || request.Permissions.Count == 0) throw new ArgumentException("Permissions are required.");
            if (request.Permissions.Any(p => string.IsNullOrWhiteSpace(p.Module))) throw new ArgumentException("Module is required.");
            return _repo.SaveAsync(request, actorUserId);
        }

        public Task DuplicateRoleAsync(string role, string newRole, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role is required.");
            if (string.IsNullOrWhiteSpace(newRole)) throw new ArgumentException("New role is required.");
            return _repo.DuplicateRoleAsync(role.Trim(), newRole.Trim(), actorUserId);
        }

        public Task<List<PermissionAuditDto>> GetAuditTrailAsync(string? role, int top)
        {
            if (top < 1) top = 100;
            if (top > 500) top = 500;
            return _repo.GetAuditTrailAsync(role, top);
        }
    }
}
