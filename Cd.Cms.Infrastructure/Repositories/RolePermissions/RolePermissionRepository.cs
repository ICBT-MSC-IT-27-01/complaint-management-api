using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Permissions;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace Cd.Cms.Infrastructure.Repositories.RolePermissions
{
    public sealed class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly IDbFactory _db;
        public RolePermissionRepository(IDbFactory db) => _db = db;

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(RolePermissionSpNames.GetRoles, conn) { CommandType = CommandType.StoredProcedure };
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();

            var list = new List<RoleDto>();
            while (await r.ReadAsync())
            {
                list.Add(new RoleDto
                {
                    Id = DataReader.GetLong(r, "Id"),
                    Role = DataReader.GetString(r, "Role"),
                    DisplayName = DataReader.GetString(r, "DisplayName"),
                    IsSystem = DataReader.GetBool(r, "IsSystem"),
                    IsActive = DataReader.GetBool(r, "IsActive")
                });
            }

            return list;
        }

        public async Task<List<RolePermissionDto>> GetByRoleAsync(string role)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(RolePermissionSpNames.GetByRole, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Role", role);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();

            var list = new List<RolePermissionDto>();
            while (await r.ReadAsync())
            {
                list.Add(new RolePermissionDto
                {
                    Module = DataReader.GetString(r, "Module"),
                    CanRead = DataReader.GetBool(r, "CanRead"),
                    CanWrite = DataReader.GetBool(r, "CanWrite"),
                    CanDelete = DataReader.GetBool(r, "CanDelete")
                });
            }

            return list;
        }

        public async Task SaveAsync(SaveRolePermissionsRequest request, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(RolePermissionSpNames.Save, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Role", request.Role);
            cmd.Parameters.AddWithValue("@PermissionsJson", JsonSerializer.Serialize(request.Permissions));
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DuplicateRoleAsync(string role, string newRole, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(RolePermissionSpNames.DuplicateRole, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@NewRole", newRole);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<PermissionAuditDto>> GetAuditTrailAsync(string? role, int top)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(RolePermissionSpNames.GetAuditTrail, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Role", (object?)role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Top", top);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();

            var list = new List<PermissionAuditDto>();
            while (await r.ReadAsync())
            {
                list.Add(new PermissionAuditDto
                {
                    Id = DataReader.GetLong(r, "Id"),
                    Role = DataReader.GetString(r, "Role"),
                    Action = DataReader.GetString(r, "Action"),
                    ChangedBy = DataReader.GetString(r, "ChangedBy"),
                    ChangedDateTime = DataReader.GetDate(r, "ChangedDateTime"),
                    Details = DataReader.GetString(r, "Details")
                });
            }

            return list;
        }
    }
}
