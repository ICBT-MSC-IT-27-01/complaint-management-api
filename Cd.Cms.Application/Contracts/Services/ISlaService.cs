using Cd.Cms.Application.DTOs.SLA;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface ISlaService
    {
        Task<List<SlaPolicyDto>> GetAllAsync();
        Task<SlaPolicyDto?> GetByIdAsync(long id);
        Task<SlaPolicyDto> CreateAsync(CreateSlaPolicyRequest request, long actorUserId);
        Task UpdateAsync(long id, CreateSlaPolicyRequest request, long actorUserId);
    }
}
