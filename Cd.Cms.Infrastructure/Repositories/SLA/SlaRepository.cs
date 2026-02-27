using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.SLA;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.SLA
{
    public class SlaRepository : ISlaRepository
    {
        private readonly IDbFactory _db;
        public SlaRepository(IDbFactory db) => _db = db;

        public async Task<List<SlaPolicyDto>> GetAllAsync()
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(SlaSpNames.GetAll, conn) { CommandType = CommandType.StoredProcedure };
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<SlaPolicyDto>();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<SlaPolicyDto?> GetByIdAsync(long id)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(SlaSpNames.GetById, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<SlaPolicyDto> CreateAsync(CreateSlaPolicyRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(SlaSpNames.Create, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CategoryId",             req.CategoryId);
            cmd.Parameters.AddWithValue("@Priority",               req.Priority);
            cmd.Parameters.AddWithValue("@ResponseTimeHours",      req.ResponseTimeHours);
            cmd.Parameters.AddWithValue("@ResolutionTimeHours",    req.ResolutionTimeHours);
            cmd.Parameters.AddWithValue("@EscalationThresholdPct", req.EscalationThresholdPct);
            cmd.Parameters.AddWithValue("@ActorUserId",            actorUserId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync()) return Map(r);
            throw new InvalidOperationException("SLA policy creation failed.");
        }

        public async Task UpdateAsync(long id, CreateSlaPolicyRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(SlaSpNames.Update, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",                     id);
            cmd.Parameters.AddWithValue("@CategoryId",             req.CategoryId);
            cmd.Parameters.AddWithValue("@Priority",               req.Priority);
            cmd.Parameters.AddWithValue("@ResponseTimeHours",      req.ResponseTimeHours);
            cmd.Parameters.AddWithValue("@ResolutionTimeHours",    req.ResolutionTimeHours);
            cmd.Parameters.AddWithValue("@EscalationThresholdPct", req.EscalationThresholdPct);
            cmd.Parameters.AddWithValue("@ActorUserId",            actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int?> GetResolutionHoursAsync(long categoryId, string priority)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(SlaSpNames.GetResolutionHrs, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@Priority",   priority);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result == null || result == DBNull.Value ? null : Convert.ToInt32(result);
        }

        private static SlaPolicyDto Map(SqlDataReader r) => new()
        {
            Id                     = DataReader.GetLong(r, "Id"),
            CategoryId             = DataReader.GetLong(r, "CategoryId"),
            CategoryName           = DataReader.GetString(r, "CategoryName"),
            Priority               = DataReader.GetString(r, "Priority"),
            ResponseTimeHours      = DataReader.GetInt(r, "ResponseTimeHours"),
            ResolutionTimeHours    = DataReader.GetInt(r, "ResolutionTimeHours"),
            EscalationThresholdPct = DataReader.GetInt(r, "EscalationThresholdPct"),
            IsActive               = DataReader.GetBool(r, "IsActive"),
        };
    }
}
