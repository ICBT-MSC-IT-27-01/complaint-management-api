namespace Cd.Cms.Application.DTOs.Reports
{
    public class ComplaintSummaryDto
    {
        public int TotalComplaints { get; set; }
        public int NewCount { get; set; }
        public int InProgressCount { get; set; }
        public int ResolvedCount { get; set; }
        public int ClosedCount { get; set; }
        public int EscalatedCount { get; set; }
        public int SlaBreachedCount { get; set; }
        public List<AgentPerformanceDto> AgentStats { get; set; } = new();
        public List<CategoryCountDto> CategoryStats { get; set; } = new();
    }

    public class AgentPerformanceDto
    {
        public long UserId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public int Assigned { get; set; }
        public int Resolved { get; set; }
        public double AvgResolutionHours { get; set; }
    }

    public class CategoryCountDto
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
