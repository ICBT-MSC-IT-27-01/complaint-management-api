namespace Cd.Cms.Application.DTOs.Complaints
{
    public class ComplaintDto
    {
        public long Id { get; set; }
        public string ComplaintNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public long ComplaintStatusId { get; set; }
        public long ComplaintChannelId { get; set; }
        public string Channel { get; set; } = string.Empty;
        public long ComplaintCategoryId { get; set; }
        public string Category { get; set; } = string.Empty;
        public long? SubCategoryId { get; set; }
        public long? ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientMobile { get; set; } = string.Empty;
        public long? AssignedToUserId { get; set; }
        public string AssignedToName { get; set; } = string.Empty;
        public DateTime? AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string SlaStatus { get; set; } = "WithinSLA";
        public bool IsSlaBreached { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string ResolutionNotes { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
