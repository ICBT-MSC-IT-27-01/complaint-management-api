namespace Cd.Cms.Application.DTOs.Complaints
{
    public class ComplaintListItemDto
    {
        public long Id { get; set; }
        public string ComplaintNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public long ComplaintStatusId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string AssignedToName { get; set; } = string.Empty;
        public string SlaStatus { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
