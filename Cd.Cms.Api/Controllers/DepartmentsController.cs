using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Departments;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/departments")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _svc;
        public DepartmentsController(IDepartmentService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] DepartmentSearchRequest req)
        {
            var result = await _svc.SearchAsync(req ?? new());
            return Ok(ApiResponse<object>.Success("Departments loaded.", result));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _svc.GetByIdAsync(id);
            return result == null ? NotFound(ApiResponse<object>.NotFound()) : Ok(ApiResponse<object>.Success("Department loaded.", result));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Typeahead([FromQuery] string q)
        {
            var result = await _svc.TypeaheadAsync(q ?? string.Empty);
            return Ok(ApiResponse<object>.Success("Search results.", result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest dto)
        {
            try
            {
                var result = await _svc.CreateAsync(dto, GetActorUserId());
                return StatusCode(201, ApiResponse<object>.Success("Department created.", result));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDepartmentRequest dto)
        {
            try
            {
                await _svc.UpdateAsync(id, dto, GetActorUserId());
                return Ok(ApiResponse<object>.Success("Department updated."));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _svc.DeleteAsync(id, GetActorUserId());
                return Ok(ApiResponse<object>.Success("Department deleted."));
            }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
    }
}
