namespace Cd.Cms.Application.DTOs.Complaints
{
    public class ResolveComplaintRequest
    {
        public string ResolutionSummary { get; set; } = string.Empty;
        public string? RootCause { get; set; }
        public string? FixApplied { get; set; }
    }
}
