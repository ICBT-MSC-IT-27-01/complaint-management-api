using Cd.Cms.Application.DTOs.Attachments;

namespace Cd.Cms.Application.Contracts.Repositories
{
    public interface IAttachmentRepository
    {
        Task<List<AttachmentDto>> GetByComplaintIdAsync(long complaintId);
        Task<AttachmentDto> SaveAsync(long complaintId, string fileName, string fileType, long fileSize, string storedPath, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
    }
}
