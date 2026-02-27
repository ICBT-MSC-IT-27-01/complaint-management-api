namespace Cd.Cms.Application.DTOs.Complaints
{
    public class AssignComplaintRequest
    {
        public long AssignedToUserId { get; set; }
        public string? Note { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
