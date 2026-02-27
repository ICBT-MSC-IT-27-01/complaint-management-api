using Cd.Cms.Application.DTOs.SLA;

namespace Cd.Cms.Application.Contracts.Repositories
{
    public interface ISlaRepository
    {
        Task<List<SlaPolicyDto>> GetAllAsync();
        Task<SlaPolicyDto?> GetByIdAsync(long id);
        Task<SlaPolicyDto> CreateAsync(CreateSlaPolicyRequest request, long actorUserId);
        Task UpdateAsync(long id, CreateSlaPolicyRequest request, long actorUserId);
        Task<int?> GetResolutionHoursAsync(long categoryId, string priority);
    }
}
