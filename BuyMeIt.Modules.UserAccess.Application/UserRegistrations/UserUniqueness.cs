using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations;
using Dapper;

namespace BuyMeIt.Modules.UserAccess.Application.UserRegistrations
{
    public class UserUniqueness : IUserUniqueness
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        private const string _sql = @"SELECT COUNT(*)
                                  FROM [users].[v_Users] AS [U]
                                  WHERE U.Login = @Login OR U.Login = @Email
                                  OR U.Email = @Login OR U.Email = @Email";
        
        public UserUniqueness(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        public bool IsUserUnique(string login, string email)
        {
            var connection = _connectionFactory.GetOpenConnection();
            

            return connection.QuerySingle<uint>(_sql, new
            {
                Login = login,
                Email = email
            }) > 0;
        }

        public async Task<bool> IsUserUniqueAsync(string login, string email)
        {
            var connection = await _connectionFactory.CreateNewConnectionAsync();

            var count = await connection.QuerySingleAsync<uint>(_sql, new
            {
                Login = login,
                Email = email
            });

            return count > 0;
        }
    }
}