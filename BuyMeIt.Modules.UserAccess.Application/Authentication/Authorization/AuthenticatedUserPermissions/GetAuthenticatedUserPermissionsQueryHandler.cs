using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.Modules.UserAccess.Application.Authentication.Authorization.UserPermissions;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace BuyMeIt.Modules.UserAccess.Application.Authentication.Authorization.AuthenticatedUserPermissions
{
    internal class GetAuthenticatedUserPermissionsQueryHandler : IQueryHandler<GetAuthenticatedUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserPermissionsQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IExecutionContextAccessor executionContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<List<UserPermissionDto>> Handle(GetAuthenticatedUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (!_executionContextAccessor.IsAvailable)
            {
                return new List<UserPermissionDto>();
            }

            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT " +
                               "[UserPermission].[PermissionCode] AS [Code] " +
                               "FROM [users].[v_UserPermissions] AS [UserPermission] " +
                               "WHERE [UserPermission].UserId = @UserId";
            var permissions = await connection.QueryAsync<UserPermissionDto>(
                sql,
                new { _executionContextAccessor.UserId });

            return permissions.AsList();
        }
    }
}