using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Attachments;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.Attachments
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly IDbFactory _db;
        public AttachmentRepository(IDbFactory db) => _db = db;

        public async Task<List<AttachmentDto>> GetByComplaintIdAsync(long complaintId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(AttachmentSpNames.GetByComplaintId, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ComplaintId", complaintId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<AttachmentDto>();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<AttachmentDto> SaveAsync(long complaintId, string fileName, string fileType, long fileSize, string storedPath, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(AttachmentSpNames.Save, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ComplaintId",  complaintId);
            cmd.Parameters.AddWithValue("@FileName",     fileName);
            cmd.Parameters.AddWithValue("@FileType",     fileType);
            cmd.Parameters.AddWithValue("@FileSizeBytes",fileSize);
            cmd.Parameters.AddWithValue("@StoredPath",   storedPath);
            cmd.Parameters.AddWithValue("@ActorUserId",  actorUserId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync()) return Map(r);
            throw new InvalidOperationException("Attachment save failed.");
        }

        public async Task DeleteAsync(long id, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(AttachmentSpNames.Delete, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",          id);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static AttachmentDto Map(SqlDataReader r) => new()
        {
            Id             = DataReader.GetLong(r, "Id"),
            ComplaintId    = DataReader.GetLong(r, "ComplaintId"),
            FileName       = DataReader.GetString(r, "FileName"),
            FileType       = DataReader.GetString(r, "FileType"),
            FileSizeBytes  = DataReader.GetLong(r, "FileSizeBytes"),
            StoredPath     = DataReader.GetString(r, "StoredPath"),
            UploadedByName = DataReader.GetString(r, "UploadedByName"),
            UploadedAt     = DataReader.GetDate(r, "UploadedAt"),
        };
    }
}
