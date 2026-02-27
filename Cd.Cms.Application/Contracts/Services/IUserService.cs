using Cd.Cms.Application.DTOs.Users;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(long id);
        Task<PagedResult<UserDto>> SearchAsync(UserSearchRequest request);
        Task<UserDto> CreateAsync(CreateUserRequest request, long actorUserId);
        Task UpdateAsync(long id, UpdateUserRequest request, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
        Task ChangePasswordAsync(long id, ChangePasswordRequest request, long actorUserId);
        Task<List<UserDto>> GetAgentsAsync();
    }
}
