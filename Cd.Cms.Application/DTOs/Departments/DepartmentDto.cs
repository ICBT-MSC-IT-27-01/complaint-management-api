namespace Cd.Cms.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
