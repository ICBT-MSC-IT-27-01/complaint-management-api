namespace Cd.Cms.Application.DTOs.SLA
{
    public class SlaPolicyDto
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int ResponseTimeHours { get; set; }
        public int ResolutionTimeHours { get; set; }
        public int EscalationThresholdPct { get; set; }
        public bool IsActive { get; set; }
    }
}
