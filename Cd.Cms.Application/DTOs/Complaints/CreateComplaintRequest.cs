namespace Cd.Cms.Application.DTOs.Complaints
{
    public class CreateComplaintRequest
    {
        public long? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientMobile { get; set; }
        public long ComplaintChannelId { get; set; }
        public long ComplaintCategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
    }
}
