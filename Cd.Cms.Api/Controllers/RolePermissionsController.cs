using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Permissions;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/role-permissions")]
    [Authorize(Roles = "Admin,SystemAdministrator")]
    public sealed class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionService _svc;
        public RolePermissionsController(IRolePermissionService svc) => _svc = svc;

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _svc.GetRolesAsync();
            return Ok(ApiResponse<object>.Success("Roles loaded.", roles));
        }

        [HttpGet("{role}")]
        public async Task<IActionResult> GetByRole(string role)
        {
            try
            {
                var permissions = await _svc.GetByRoleAsync(role);
                return Ok(ApiResponse<object>.Success("Permissions loaded.", permissions));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveRolePermissionsRequest request)
        {
            try
            {
                await _svc.SaveAsync(request, GetActorUserId());
                return Ok(ApiResponse<object>.Success("Permissions saved."));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpPost("{role}/duplicate/{newRole}")]
        public async Task<IActionResult> DuplicateRole(string role, string newRole)
        {
            try
            {
                await _svc.DuplicateRoleAsync(role, newRole, GetActorUserId());
                return Ok(ApiResponse<object>.Success("Role duplicated."));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpGet("audit-trail")]
        public async Task<IActionResult> GetAuditTrail([FromQuery] string? role, [FromQuery] int top = 100)
        {
            var records = await _svc.GetAuditTrailAsync(role, top);
            return Ok(ApiResponse<object>.Success("Audit trail loaded.", records));
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
    }
}
