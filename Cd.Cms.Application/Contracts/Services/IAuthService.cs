using Cd.Cms.Application.DTOs.Users;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
        Task<ClientEmailCheckResponseDto> CheckClientEmailAsync(string email, CancellationToken ct = default);
        Task<AuthResponseDto> RegisterClientAsync(ClientRegisterRequestDto request, CancellationToken ct = default);
    }
}
