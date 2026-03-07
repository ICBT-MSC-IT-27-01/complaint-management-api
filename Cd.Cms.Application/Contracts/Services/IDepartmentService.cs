using Cd.Cms.Application.DTOs.Departments;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Contracts.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentDto?> GetByIdAsync(long id);
        Task<PagedResult<DepartmentDto>> SearchAsync(DepartmentSearchRequest request);
        Task<List<DepartmentDto>> TypeaheadAsync(string q);
        Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request, long actorUserId);
        Task UpdateAsync(long id, UpdateDepartmentRequest request, long actorUserId);
        Task DeleteAsync(long id, long actorUserId);
    }
}
