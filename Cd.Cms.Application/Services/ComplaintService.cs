using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Complaints;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _repo;

        public ComplaintService(IComplaintRepository repo) => _repo = repo;

        public Task<ComplaintDto?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<ComplaintListItemDto>> SearchAsync(ComplaintSearchRequest request) => _repo.SearchAsync(request);

        public Task<ComplaintDto> CreateAsync(CreateComplaintRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.Subject)) throw new ArgumentException("Subject is required.");
            if (string.IsNullOrWhiteSpace(request.Description)) throw new ArgumentException("Description is required.");
            if (string.IsNullOrWhiteSpace(request.Priority)) throw new ArgumentException("Priority is required.");
            return _repo.CreateAsync(request, actorUserId);
        }

        public Task AssignAsync(long id, AssignComplaintRequest request, long actorUserId)
        {
            if (request.AssignedToUserId <= 0) throw new ArgumentException("AssignedToUserId is required.");
            return _repo.AssignAsync(id, request, actorUserId);
        }

        public Task UpdateStatusAsync(long id, UpdateComplaintStatusRequest request, long actorUserId)
        {
            if (request.StatusId <= 0) throw new ArgumentException("StatusId is required.");
            return _repo.UpdateStatusAsync(id, request, actorUserId);
        }

        public Task EscalateAsync(long id, EscalateComplaintRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("Escalation reason is required.");
            if (request.EscalatedToUserId <= 0) throw new ArgumentException("EscalatedToUserId is required.");
            return _repo.EscalateAsync(id, request, actorUserId);
        }

        public Task ResolveAsync(long id, ResolveComplaintRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.ResolutionSummary)) throw new ArgumentException("Resolution summary is required.");
            return _repo.ResolveAsync(id, request, actorUserId);
        }

        public Task CloseAsync(long id, long actorUserId) => _repo.CloseAsync(id, actorUserId);
        public Task DeleteAsync(long id, long actorUserId) => _repo.DeleteAsync(id, actorUserId);
        public Task<List<ComplaintHistoryDto>> GetHistoryAsync(long id) => _repo.GetHistoryAsync(id);
    }
}
