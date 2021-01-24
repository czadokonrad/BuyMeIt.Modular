using System.Data;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Application.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
        Task<IDbConnection> GetOpenConnectionAsync();

        IDbConnection CreateNewConnection();
        Task<IDbConnection> CreateNewConnectionAsync();

        string GetConnectionString();
    }
}
