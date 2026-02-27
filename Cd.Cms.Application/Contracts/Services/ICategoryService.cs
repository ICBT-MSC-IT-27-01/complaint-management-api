using Cd.Cms.Application.DTOs.Categories;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(long id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request, long actorUserId);
        Task UpdateAsync(long id, CreateCategoryRequest request, long actorUserId);
        Task DeactivateAsync(long id, long actorUserId);
    }
}
