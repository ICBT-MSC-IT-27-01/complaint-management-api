using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Departments;
using Cd.Cms.Shared;

namespace Cd.Cms.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repo;
        public DepartmentService(IDepartmentRepository repo) => _repo = repo;

        public Task<DepartmentDto?> GetByIdAsync(long id) => _repo.GetByIdAsync(id);
        public Task<PagedResult<DepartmentDto>> SearchAsync(DepartmentSearchRequest request) => _repo.SearchAsync(request);
        public Task<List<DepartmentDto>> TypeaheadAsync(string q) => _repo.TypeaheadAsync(q);

        public Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("Department name is required.");
            return _repo.CreateAsync(request, actorUserId);
        }

        public Task UpdateAsync(long id, UpdateDepartmentRequest request, long actorUserId)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("Department name is required.");
            return _repo.UpdateAsync(id, request, actorUserId);
        }

        public Task DeleteAsync(long id, long actorUserId) => _repo.DeleteAsync(id, actorUserId);
    }
}
