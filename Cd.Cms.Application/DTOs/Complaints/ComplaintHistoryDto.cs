namespace Cd.Cms.Application.DTOs.Complaints
{
    public class ComplaintHistoryDto
    {
        public long Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? Note { get; set; }
        public string PerformedByName { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; }
    }
}
