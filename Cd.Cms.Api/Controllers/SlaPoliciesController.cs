using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.SLA;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/sla-policies")]
    [Authorize]
    public class SlaPoliciesController : ControllerBase
    {
        private readonly ISlaService _svc;
        public SlaPoliciesController(ISlaService svc) => _svc = svc;

        [HttpGet]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _svc.GetAllAsync();
            return Ok(ApiResponse<object>.Success("SLA Policies loaded.", result));
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _svc.GetByIdAsync(id);
            return result == null ? NotFound(ApiResponse<object>.NotFound()) : Ok(ApiResponse<object>.Success("SLA Policy loaded.", result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSlaPolicyRequest dto)
        {
            try
            {
                var result = await _svc.CreateAsync(dto, GetActorUserId());
                return StatusCode(201, ApiResponse<object>.Success("SLA Policy created.", result));
            }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateSlaPolicyRequest dto)
        {
            try { await _svc.UpdateAsync(id, dto, GetActorUserId()); return Ok(ApiResponse<object>.Success("SLA Policy updated.")); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
    }
}
