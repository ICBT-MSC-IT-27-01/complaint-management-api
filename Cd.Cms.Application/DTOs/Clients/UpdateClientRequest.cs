namespace Cd.Cms.Application.DTOs.Clients
{
    public class UpdateClientRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string PrimaryEmail { get; set; } = string.Empty;
        public string? PrimaryPhone { get; set; }
        public string? Address { get; set; }
        public string ClientType { get; set; } = "Standard";
        public long? AccountManagerId { get; set; }
    }
}
