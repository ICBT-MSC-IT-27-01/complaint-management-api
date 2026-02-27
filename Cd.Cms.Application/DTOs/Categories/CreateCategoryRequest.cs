namespace Cd.Cms.Application.DTOs.Categories
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public long? ParentCategoryId { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}
