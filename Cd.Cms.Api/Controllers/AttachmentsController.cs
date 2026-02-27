using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/complaints/{complaintId:long}/attachments")]
    [Authorize]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _svc;
        public AttachmentsController(IAttachmentService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll(long complaintId)
        {
            var result = await _svc.GetByComplaintIdAsync(complaintId);
            return Ok(ApiResponse<object>.Success("Attachments loaded.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(long complaintId, IFormFile file)
        {
            try
            {
                var result = await _svc.UploadAsync(complaintId, file, GetActorUserId());
                return StatusCode(201, ApiResponse<object>.Success("File uploaded.", result));
            }
            catch (ArgumentException ex) { return BadRequest(ApiResponse<object>.ValidationError(ex.Message)); }
            catch (Exception ex)         { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Delete(long complaintId, long id)
        {
            try { await _svc.DeleteAsync(id, GetActorUserId()); return Ok(ApiResponse<object>.Success("Attachment deleted.")); }
            catch (Exception ex) { return StatusCode(500, ApiResponse<object>.Error(ex.Message)); }
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
    }
}
