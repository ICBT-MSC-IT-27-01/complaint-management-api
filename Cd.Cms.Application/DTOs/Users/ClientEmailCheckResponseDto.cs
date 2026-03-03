namespace Cd.Cms.Application.DTOs.Users
{
    public sealed class ClientEmailCheckResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public bool EmailExists { get; set; }
        public bool RequiresPassword { get; set; }
        public bool RequiresRegistration { get; set; }
    }
}
