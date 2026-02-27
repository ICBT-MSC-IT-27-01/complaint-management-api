namespace Cd.Cms.Application.DTOs.Cases
{
    public class UpdateCaseStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
