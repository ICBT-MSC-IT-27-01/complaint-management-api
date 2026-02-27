namespace Cd.Cms.Application.DTOs.Clients
{
    public class ClientDto
    {
        public long Id { get; set; }
        public string ClientCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string PrimaryEmail { get; set; } = string.Empty;
        public string PrimaryPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ClientType { get; set; } = string.Empty;
        public long? AccountManagerId { get; set; }
        public string AccountManagerName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
