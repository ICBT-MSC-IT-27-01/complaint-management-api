namespace Cd.Cms.Application.DTOs.Clients
{
    public class ClientSearchRequest
    {
        public string? Q { get; set; }
        public string? ClientType { get; set; }
        public bool? IsActive { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
