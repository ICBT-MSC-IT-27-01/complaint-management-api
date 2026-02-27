namespace Cd.Cms.Application.DTOs.Complaints
{
    public class UpdateComplaintStatusRequest
    {
        public long StatusId { get; set; }
        public string? Note { get; set; }
    }
}
