using Cd.Cms.Application.DTOs.Cases;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface ICaseService
    {
        Task<CaseDto?> GetByIdAsync(long id);
        Task<CaseDto?> GetByComplaintIdAsync(long complaintId);
        Task<List<CaseActivityDto>> GetActivitiesAsync(long caseId);
        Task AddActivityAsync(long caseId, AddCaseActivityRequest request, long actorUserId);
        Task UpdateStatusAsync(long caseId, UpdateCaseStatusRequest request, long actorUserId);
    }
}
