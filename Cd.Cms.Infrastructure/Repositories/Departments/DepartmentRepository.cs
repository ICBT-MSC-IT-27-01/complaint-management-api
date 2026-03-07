using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Departments;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDbFactory _db;
        public DepartmentRepository(IDbFactory db) => _db = db;

        public async Task<DepartmentDto?> GetByIdAsync(long id)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.GetById, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<PagedResult<DepartmentDto>> SearchAsync(DepartmentSearchRequest req)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.Search, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Q", (object?)req.Q ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object?)req.IsActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page", req.Page);
            cmd.Parameters.AddWithValue("@PageSize", req.PageSize);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var items = new List<DepartmentDto>();
            while (await r.ReadAsync()) items.Add(Map(r));
            long total = 0;
            if (await r.NextResultAsync() && await r.ReadAsync()) total = r.GetInt64(0);
            return new PagedResult<DepartmentDto> { Page = req.Page, PageSize = req.PageSize, TotalCount = total, Items = items.ToArray() };
        }

        public async Task<List<DepartmentDto>> TypeaheadAsync(string q)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.Typeahead, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Q", q);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<DepartmentDto>();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.Create, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Name", req.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)req.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder", req.SortOrder);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync()) return Map(r);
            throw new InvalidOperationException("Department creation failed.");
        }

        public async Task UpdateAsync(long id, UpdateDepartmentRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.Update, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", req.Name);
            cmd.Parameters.AddWithValue("@Description", (object?)req.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder", req.SortOrder);
            cmd.Parameters.AddWithValue("@IsActive", req.IsActive);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(long id, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(DepartmentSpNames.Delete, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static DepartmentDto Map(SqlDataReader r) => new()
        {
            Id = DataReader.GetLong(r, "Id"),
            DepartmentCode = DataReader.GetString(r, "DepartmentCode"),
            Name = DataReader.GetString(r, "Name"),
            Description = DataReader.GetString(r, "Description"),
            SortOrder = DataReader.GetInt(r, "SortOrder"),
            IsActive = DataReader.GetBool(r, "IsActive"),
            CreatedDateTime = DataReader.GetDate(r, "CreatedDateTime")
        };
    }
}
