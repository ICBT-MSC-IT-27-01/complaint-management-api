using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Categories;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbFactory _db;
        public CategoryRepository(IDbFactory db) => _db = db;

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CategorySpNames.GetAll, conn) { CommandType = CommandType.StoredProcedure };
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var all = new List<CategoryDto>();
            while (await r.ReadAsync()) all.Add(MapFlat(r));
            // Build tree
            var lookup = all.ToDictionary(x => x.Id);
            var roots = new List<CategoryDto>();
            foreach (var cat in all)
            {
                if (cat.ParentCategoryId.HasValue && lookup.TryGetValue(cat.ParentCategoryId.Value, out var parent))
                    parent.Children.Add(cat);
                else roots.Add(cat);
            }
            return roots;
        }

        public async Task<CategoryDto?> GetByIdAsync(long id)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CategorySpNames.GetById, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? MapFlat(r) : null;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CategorySpNames.Create, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Name",             req.Name);
            cmd.Parameters.AddWithValue("@ParentCategoryId", (object?)req.ParentCategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder",        req.SortOrder);
            cmd.Parameters.AddWithValue("@ActorUserId",      actorUserId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync()) return MapFlat(r);
            throw new InvalidOperationException("Category creation failed.");
        }

        public async Task UpdateAsync(long id, CreateCategoryRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CategorySpNames.Update, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",               id);
            cmd.Parameters.AddWithValue("@Name",             req.Name);
            cmd.Parameters.AddWithValue("@ParentCategoryId", (object?)req.ParentCategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder",        req.SortOrder);
            cmd.Parameters.AddWithValue("@ActorUserId",      actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeactivateAsync(long id, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CategorySpNames.Deactivate, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",          id);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static CategoryDto MapFlat(SqlDataReader r) => new()
        {
            Id               = DataReader.GetLong(r, "Id"),
            Name             = DataReader.GetString(r, "Name"),
            ParentCategoryId = DataReader.GetNullableLong(r, "ParentCategoryId"),
            ParentName       = DataReader.GetString(r, "ParentName"),
            SortOrder        = DataReader.GetInt(r, "SortOrder"),
            IsActive         = DataReader.GetBool(r, "IsActive"),
        };
    }
}
