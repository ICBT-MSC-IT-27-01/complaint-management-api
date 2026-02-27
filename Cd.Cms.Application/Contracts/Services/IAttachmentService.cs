using Cd.Cms.Application.DTOs.Attachments;
using Microsoft.AspNetCore.Http;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IAttachmentService
    {
        Task<List<AttachmentDto>> GetByComplaintIdAsync(long complaintId);
        Task<AttachmentDto> UploadAsync(long complaintId, IFormFile file, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
    }
}
