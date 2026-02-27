using System.Data;

namespace Cd.Cms.Infrastructure.Contracts
{
    public interface IDbFactory
    {
        IDbConnection CreateConnection();
    }
}
