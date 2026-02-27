using Cd.Cms.Application.DTOs.Complaints;
using Cd.Cms.Application.DTOs.Users;
using Cd.Cms.Shared;

namespace Cd.Cms.Tests.Helpers
{
    public static class MockHelper
    {
        public static UserDto SampleUser(long id = 1, string role = "Agent") => new()
        { Id = id, Name = "Test User", Email = "test@cms.com", Username = "testuser", Role = role, IsActive = true };

        public static ComplaintDto SampleComplaint(long id = 1) => new()
        {
            Id = id, ComplaintNumber = "CMP-202600001", Subject = "Test complaint",
            Description = "Test description", Priority = "High", Status = "New",
            ComplaintStatusId = 1, IsActive = true, CreatedDateTime = DateTime.UtcNow
        };

        public static CreateComplaintRequest SampleCreateRequest() => new()
        { Subject = "Test complaint", Description = "Test description", Priority = "Medium", ComplaintChannelId = 1, ComplaintCategoryId = 1 };

        public static PagedResult<ComplaintListItemDto> SamplePagedComplaints(int count = 3) => new()
        {
            Page = 1, PageSize = 20, TotalCount = count,
            Items = Enumerable.Range(1, count).Select(i => new ComplaintListItemDto
            { Id = i, ComplaintNumber = $"CMP-20260000{i}", Subject = $"Complaint {i}", Priority = "Medium", Status = "New" }).ToArray()
        };
    }
}
