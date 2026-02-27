using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Clients;
using Cd.Cms.Infrastructure.Contracts;
using Cd.Cms.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cd.Cms.Infrastructure.Repositories.Clients
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDbFactory _db;
        public ClientRepository(IDbFactory db) => _db = db;

        public async Task<ClientDto?> GetByIdAsync(long id)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.GetById, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            return await r.ReadAsync() ? Map(r) : null;
        }

        public async Task<PagedResult<ClientDto>> SearchAsync(ClientSearchRequest req)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.Search, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Q",          (object?)req.Q          ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientType", (object?)req.ClientType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive",   (object?)req.IsActive   ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Page",       req.Page);
            cmd.Parameters.AddWithValue("@PageSize",   req.PageSize);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var items = new List<ClientDto>();
            while (await r.ReadAsync()) items.Add(Map(r));
            long total = 0;
            if (await r.NextResultAsync() && await r.ReadAsync()) total = r.GetInt64(0);
            return new PagedResult<ClientDto> { Page = req.Page, PageSize = req.PageSize, TotalCount = total, Items = items.ToArray() };
        }

        public async Task<List<ClientDto>> TypeaheadAsync(string q)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.Typeahead, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Q", q);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<ClientDto>();
            while (await r.ReadAsync()) list.Add(Map(r));
            return list;
        }

        public async Task<ClientDto> CreateAsync(CreateClientRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.Create, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@CompanyName",      req.CompanyName);
            cmd.Parameters.AddWithValue("@PrimaryEmail",     req.PrimaryEmail);
            cmd.Parameters.AddWithValue("@PrimaryPhone",     (object?)req.PrimaryPhone    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address",          (object?)req.Address         ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientType",       req.ClientType);
            cmd.Parameters.AddWithValue("@AccountManagerId", (object?)req.AccountManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ActorUserId",      actorUserId);
            await conn.OpenAsync();
            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync()) return Map(r);
            throw new InvalidOperationException("Client creation failed.");
        }

        public async Task UpdateAsync(long id, UpdateClientRequest req, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.Update, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",               id);
            cmd.Parameters.AddWithValue("@CompanyName",      req.CompanyName);
            cmd.Parameters.AddWithValue("@PrimaryEmail",     req.PrimaryEmail);
            cmd.Parameters.AddWithValue("@PrimaryPhone",     (object?)req.PrimaryPhone    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address",          (object?)req.Address         ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientType",       req.ClientType);
            cmd.Parameters.AddWithValue("@AccountManagerId", (object?)req.AccountManagerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ActorUserId",      actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(long id, long actorUserId)
        {
            using var conn = (SqlConnection)_db.CreateConnection();
            using var cmd = new SqlCommand(ClientSpNames.Delete, conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Id",          id);
            cmd.Parameters.AddWithValue("@ActorUserId", actorUserId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static ClientDto Map(SqlDataReader r) => new()
        {
            Id                 = DataReader.GetLong(r, "Id"),
            ClientCode         = DataReader.GetString(r, "ClientCode"),
            CompanyName        = DataReader.GetString(r, "CompanyName"),
            PrimaryEmail       = DataReader.GetString(r, "PrimaryEmail"),
            PrimaryPhone       = DataReader.GetString(r, "PrimaryPhone"),
            Address            = DataReader.GetString(r, "Address"),
            ClientType         = DataReader.GetString(r, "ClientType"),
            AccountManagerId   = DataReader.GetNullableLong(r, "AccountManagerId"),
            AccountManagerName = DataReader.GetString(r, "AccountManagerName"),
            IsActive           = DataReader.GetBool(r, "IsActive"),
            CreatedDateTime    = DataReader.GetDate(r, "CreatedDateTime"),
        };
    }
}
