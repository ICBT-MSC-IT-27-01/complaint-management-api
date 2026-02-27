namespace Cd.Cms.Application.DTOs.Cases
{
    public class CaseActivityDto
    {
        public long Id { get; set; }
        public long CaseId { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PerformedByName { get; set; } = string.Empty;
        public long PerformedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
