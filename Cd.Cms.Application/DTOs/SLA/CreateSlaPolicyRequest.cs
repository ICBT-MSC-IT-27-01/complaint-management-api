namespace Cd.Cms.Application.DTOs.SLA
{
    public class CreateSlaPolicyRequest
    {
        public long CategoryId { get; set; }
        public string Priority { get; set; } = string.Empty;
        public int ResponseTimeHours { get; set; }
        public int ResolutionTimeHours { get; set; }
        public int EscalationThresholdPct { get; set; } = 80;
    }
}
