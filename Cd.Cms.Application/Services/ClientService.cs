using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Clients;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        public ClientService(IClientRepository repo) => _repo = repo;

        public Task<ClientDto?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<ClientDto>> SearchAsync(ClientSearchRequest request) => _repo.SearchAsync(request);
        public Task<List<ClientDto>> TypeaheadAsync(string q) => _repo.TypeaheadAsync(q);
        public Task<ClientDto> CreateAsync(CreateClientRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.CompanyName)) throw new ArgumentException("Company name is required.");
            if (string.IsNullOrWhiteSpace(request.PrimaryEmail)) throw new ArgumentException("Email is required.");
            return _repo.CreateAsync(request, actorUserId);
        }
        public Task UpdateAsync(long id, UpdateClientRequest request, long actorUserId) => _repo.UpdateAsync(id, request, actorUserId);
        public Task DeleteAsync(long id, long actorUserId) => _repo.DeleteAsync(id, actorUserId);
    }
}
