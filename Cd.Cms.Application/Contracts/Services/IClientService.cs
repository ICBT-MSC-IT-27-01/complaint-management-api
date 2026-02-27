using Cd.Cms.Application.DTOs.Clients;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IClientService
    {
        Task<ClientDto?> GetByIdAsync(long id);
        Task<PagedResult<ClientDto>> SearchAsync(ClientSearchRequest request);
        Task<List<ClientDto>> TypeaheadAsync(string q);
        Task<ClientDto> CreateAsync(CreateClientRequest request, long actorUserId);
        Task UpdateAsync(long id, UpdateClientRequest request, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
    }
}
