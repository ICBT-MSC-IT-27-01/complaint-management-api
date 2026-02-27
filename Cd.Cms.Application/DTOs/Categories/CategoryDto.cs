namespace Cd.Cms.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? ParentCategoryId { get; set; }
        public string? ParentName { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryDto> Children { get; set; } = new();
    }
}
