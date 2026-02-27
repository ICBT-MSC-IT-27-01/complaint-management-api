using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Cases;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cases")]
    [Authorize]
    public class CasesController : ControllerBase
    {
        private readonly ICaseService _svc;
        public CasesController(ICaseService svc) => _svc = svc;

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _svc.GetByIdAsync(id);
            return result == null ? NotFound(ApiResponse<object>.NotFound()) : Ok(ApiResponse<object>.Success("Case loaded.", result));
        }

        [HttpGet("{id:long}/activities")]
        public async Task<IActionResult> GetActivities(long id)
        {
            var result = await _svc.GetActivitiesAsync(id);
            return Ok(ApiResponse<object>.Success("Activities loaded.", result));
        }

        [HttpPost("{id:long}/activities")]
        [Authorize(Roles = "Admin,Supervisor,Agent")]
        public async Task<IActionResult> AddActivity(long id, [FromBody] AddCaseActivityRequest dto)
        {
            try { await _svc.AddActivityAsync(id, dto, GetActorUserId()); return Ok(ApiResponse<object>.Success("Activity logged.")); }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex)         { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpPatch("{id:long}/status")]
        [Authorize(Roles = "Admin,Supervisor,Agent")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateCaseStatusRequest dto)
        {
            try { await _svc.UpdateStatusAsync(id, dto, GetActorUserId()); return Ok(ApiResponse<object>.Success("Case status updated.")); }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex)         { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
    }
}
