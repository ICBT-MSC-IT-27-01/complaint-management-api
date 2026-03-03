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
            if (request == null)
                throw new ArgumentException("Request body is required.");
            if (string.IsNullOrWhiteSpace(request.EmailOrUsername))
                throw new ArgumentException("Email or username is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            var user = await _users.GetAuthUserByEmailOrUsernameAsync(request.EmailOrUsername)
                ?? throw new UnauthorizedAccessException("Invalid credentials.");

            if (!user.IsActive)
                throw new InvalidOperationException("Account is inactive.");
            if (user.IsLocked)
                throw new InvalidOperationException("Account is locked.");

            if (!IsPasswordValid(user.PasswordHash, request.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return GenerateAuthResponse(user);
        }

        public async Task<ClientEmailCheckResponseDto> CheckClientEmailAsync(string email, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            var normalizedEmail = email.Trim();
            var user = await _users.GetAuthUserByEmailOrUsernameAsync(normalizedEmail);
            var exists = user != null && user.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase);

            return new ClientEmailCheckResponseDto
            {
                Email = normalizedEmail,
                EmailExists = exists,
                RequiresPassword = exists,
                RequiresRegistration = !exists
            };
        }

        public async Task<AuthResponseDto> RegisterClientAsync(ClientRegisterRequestDto request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentException("Request body is required.");
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");
            if (request.Password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
                throw new ArgumentException("Passwords do not match.");

            var normalizedEmail = request.Email.Trim();
            var existing = await _users.GetAuthUserByEmailOrUsernameAsync(normalizedEmail);
            if (existing != null && existing.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Email is already registered.");

            var username = await GenerateUniqueClientUsernameAsync(normalizedEmail);
            var hashedPassword = _hasher.HashPassword(new User(), request.Password);

            UserDto created;
            try
            {
                created = await _users.CreateAsync(new CreateUserRequest
                {
                    Name = request.Name.Trim(),
                    Email = normalizedEmail,
                    Username = username,
                    PhoneNumber = request.PhoneNumber?.Trim() ?? string.Empty,
                    Password = hashedPassword,
                    Role = "Client"
                }, actorUserId: 0);
            }
            catch (Exception ex) when (ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            return GenerateAuthResponse(new AuthUserDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email,
                Username = created.Username,
                Role = created.Role,
                IsActive = created.IsActive,
                IsLocked = created.IsLocked,
                PasswordHash = hashedPassword
            });
        }

        private bool IsPasswordValid(string storedPasswordHash, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(storedPasswordHash)) return false;

            try
            {
                var passwordCheck = _hasher.VerifyHashedPassword(new User(), storedPasswordHash, providedPassword);
                return passwordCheck != PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                // Fallback for legacy/plaintext seeded data that is not in ASP.NET Identity hash format.
                return string.Equals(storedPasswordHash, providedPassword, StringComparison.Ordinal);
            }
        }

        private async Task<string> GenerateUniqueClientUsernameAsync(string email)
        {
            var baseUsername = email.Split('@')[0].Trim();
            if (string.IsNullOrWhiteSpace(baseUsername)) baseUsername = "client";

            var cleaned = new string(baseUsername.Where(c => char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-').ToArray());
            if (string.IsNullOrWhiteSpace(cleaned)) cleaned = "client";

            var candidate = cleaned;
            var suffix = 1;
            while (await _users.GetByEmailOrUsernameAsync(candidate) != null)
            {
                candidate = $"{cleaned}{suffix}";
                suffix++;
            }

            return candidate;
        }

        private AuthResponseDto GenerateAuthResponse(AuthUserDto user)
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
