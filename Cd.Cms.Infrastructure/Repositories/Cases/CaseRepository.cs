using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Cases;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.Cases
{
    public class CaseRepository : ICaseRepository
    {
        private readonly IDbFactory _db;
        public CaseRepository(IDbFactory db) => _db = db;

        public async Task<CaseDto?> GetByIdAsync(long id)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CaseSpNames.GetById, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CaseId", id);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? MapCase(r) : null;
        }

        public async Task<CaseDto?> GetByComplaintIdAsync(long complaintId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CaseSpNames.GetByComplaintId, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ComplaintId", complaintId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? MapCase(r) : null;
        }

        public async Task<List<CaseActivityDto>> GetActivitiesAsync(long caseId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CaseSpNames.GetActivities, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CaseId", caseId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<CaseActivityDto>();
            while (await r.ReadAsync())
                list.Add(new CaseActivityDto {
                    Id                 = DataReader.GetLong(r, "Id"),
                    CaseId             = DataReader.GetLong(r, "CaseId"),
                    ActivityType       = DataReader.GetString(r, "ActivityType"),
                    Description        = DataReader.GetString(r, "Description"),
                    PerformedByUserId  = DataReader.GetLong(r, "PerformedByUserId"),
                    PerformedByName    = DataReader.GetString(r, "PerformedByName"),
                    CreatedDateTime    = DataReader.GetDate(r, "CreatedDateTime"),
                });
            return list;
        }

        public async Task AddActivityAsync(long caseId, AddCaseActivityRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CaseSpNames.AddActivity, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CaseId",       caseId);
            cmd.Parameters.AddWithValue("@ActivityType", req.ActivityType);
            cmd.Parameters.AddWithValue("@Description",  req.Description);
            cmd.Parameters.AddWithValue("@ActorUserId",  actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateStatusAsync(long caseId, UpdateCaseStatusRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(CaseSpNames.UpdateStatus, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CaseId",      caseId);
            cmd.Parameters.AddWithValue("@Status",      req.Status);
            cmd.Parameters.AddWithValue("@Note",        (object?)req.Note ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static CaseDto MapCase(SqlDataReader r) => new()
        {
            Id               = DataReader.GetLong(r, "Id"),
            CaseNumber       = DataReader.GetString(r, "CaseNumber"),
            ComplaintId      = DataReader.GetLong(r, "ComplaintId"),
            ComplaintNumber  = DataReader.GetString(r, "ComplaintNumber"),
            Status           = DataReader.GetString(r, "Status"),
            AssignedToUserId = DataReader.GetNullableLong(r, "AssignedToUserId"),
            AssignedToName   = DataReader.GetString(r, "AssignedToName"),
            Notes            = DataReader.GetString(r, "Notes"),
            OpenedAt         = DataReader.GetDate(r, "OpenedAt"),
            ClosedAt         = DataReader.GetNullableDate(r, "ClosedAt"),
        };
    }
}
