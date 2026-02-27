namespace Cd.Cms.Application.DTOs.Cases
{
    public class CaseDto
    {
        public long Id { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public long ComplaintId { get; set; }
        public string ComplaintNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public long? AssignedToUserId { get; set; }
        public string AssignedToName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
