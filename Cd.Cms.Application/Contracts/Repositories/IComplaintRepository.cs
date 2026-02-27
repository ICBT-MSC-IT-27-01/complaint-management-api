using Cd.Cms.Application.DTOs.Complaints;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Contracts.Repositories
{
    public interface IComplaintRepository
    {
        Task<ComplaintDto?> GetByIdAsync(long id);
        Task<PagedResult<ComplaintListItemDto>> SearchAsync(ComplaintSearchRequest request);
        Task<ComplaintDto> CreateAsync(CreateComplaintRequest request, long actorUserId);
        Task UpdateAsync(long id, CreateComplaintRequest request, long actorUserId);
        Task AssignAsync(long id, AssignComplaintRequest request, long actorUserId);
        Task UpdateStatusAsync(long id, UpdateComplaintStatusRequest request, long actorUserId);
        Task EscalateAsync(long id, EscalateComplaintRequest request, long actorUserId);
        Task ResolveAsync(long id, ResolveComplaintRequest request, long actorUserId);
        Task CloseAsync(long id, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
        Task<List<ComplaintHistoryDto>> GetHistoryAsync(long id);
    }
}
