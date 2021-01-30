using BuyMeIt.BuildingBlocks.Application.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure
{
    public sealed class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateNewConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            return connection;
        }

        public async Task<IDbConnection> CreateNewConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            return connection;
        }

        public string GetConnectionString() => _connectionString;

        public IDbConnection GetOpenConnection()
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                this._connection = new SqlConnection(_connectionString);
                this._connection.Open();
            }

            return this._connection;
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            if (this._connection == null || this._connection.State != ConnectionState.Open)
            {
                var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                this._connection = connection;
            }

            return this._connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _connection = null;
        }
    }
}
