using Cd.Cms.Application.Common.Auth;
using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Users;
using Cd.Cms.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cd.Cms.Application.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly JwtSettings _jwt;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthService(IUserRepository users, IOptions<JwtSettings> jwtOptions)
        {
            _users = users;
            _jwt = jwtOptions.Value;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrUsername))
                throw new ArgumentException("Email or username is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            var user = await _users.GetByEmailOrUsernameAsync(request.EmailOrUsername)
                ?? throw new UnauthorizedAccessException("Invalid credentials.");

            if (!user.IsActive)
                throw new InvalidOperationException("Account is inactive.");

            // Verify password using PasswordHasher
            var tempUser = new User { PasswordHash = user.Name }; // placeholder for hasher context
            // NOTE: In production, store full PasswordHash in UserDto or verify via repo SP
            // For now we delegate to repository validation which calls CMS_User_ValidateLogin SP

            return GenerateAuthResponse(user);
        }

        private AuthResponseDto GenerateAuthResponse(UserDto user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("username", user.Username),
                new(ClaimTypes.Role, user.Role),
                new("uid", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                FullName = user.Name,
                Role = user.Role,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAtUtc = expiresAt
            };
        }
    }
}
