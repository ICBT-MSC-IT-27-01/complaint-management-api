using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.SLA;

namespace Cd.Cms.Application.Services
{
    public class SlaService : ISlaService
    {
        private readonly ISlaRepository _repo;
        public SlaService(ISlaRepository repo) => _repo = repo;

        public Task<List<SlaPolicyDto>> GetAllAsync() => _repo.GetAllAsync();
        public Task<SlaPolicyDto?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
        public Task<SlaPolicyDto> CreateAsync(CreateSlaPolicyRequest request, long actorUserId) => _repo.CreateAsync(request, actorUserId);
        public Task UpdateAsync(long id, CreateSlaPolicyRequest request, long actorUserId) => _repo.UpdateAsync(id, request, actorUserId);
    }
}
