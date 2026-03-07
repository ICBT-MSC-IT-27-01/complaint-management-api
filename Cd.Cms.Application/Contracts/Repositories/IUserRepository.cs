using Cd.Cms.Application.DTOs.Users;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByIdAsync(long id);
        Task<UserDto?> GetByEmailOrUsernameAsync(string emailOrUsername);
        Task<AuthUserDto?> GetAuthUserByEmailOrUsernameAsync(string emailOrUsername);
        Task<PagedResult<UserDto>> SearchAsync(UserSearchRequest request);
        Task<UserDto> CreateAsync(CreateUserRequest request, long actorUserId);
        Task UpdateAsync(long id, UpdateUserRequest request, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
        Task ChangePasswordAsync(long id, string newPasswordHash, long actorUserId);
        Task<bool> SetTemporaryPasswordAndSendEmailAsync(
            long id,
            string email,
            string temporaryPassword,
            string temporaryPasswordHash,
            long actorUserId);
        Task<List<UserDto>> GetAgentsAsync();
    }
}
