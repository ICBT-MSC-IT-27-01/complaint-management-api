using Cd.Cms.Infrastructure.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Cd.Cms.Infrastructure.Persistence
{
    public class DbFactory : IDbFactory
    {
        private readonly string _cs;

        public DbFactory(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_cs);
    }
}
