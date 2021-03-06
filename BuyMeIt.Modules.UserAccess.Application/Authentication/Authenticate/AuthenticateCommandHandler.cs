﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Commands;
using BuyMeIt.Modules.UserAccess.Application.Contracts;
using Dapper;

namespace BuyMeIt.Modules.UserAccess.Application.Authentication.Authenticate
{
    public class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AuthenticateCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var connection = await _sqlConnectionFactory.GetOpenConnectionAsync();

            const string sql = "SELECT " +
                               "[User].[Id], " +
                               "[User].[Login], " +
                               "[User].[Name], " +
                               "[User].[Email], " +
                               "[User].[IsActive], " +
                               "[User].[Password] " +
                               "FROM [users].[v_Users] AS [User] " +
                               "WHERE [User].[Login] = @Login";

            var user = await connection.QuerySingleOrDefaultAsync<UserDto>(
                sql,
                new
                {
                    request.Login,
                });

            if (user == null)
            {
                return new AuthenticationResult("Incorrect login or password");
            }

            if (!user.IsActive)
            {
                return new AuthenticationResult("User is not active");
            }

            if (!PasswordManager.VerifyHashedPassword(user.Password, request.Password))
            {
                return new AuthenticationResult("Incorrect login or password");
            }

            user.Claims = new List<Claim>();
            user.Claims.Add(new Claim(CustomClaimTypes.Name, user.Name));
            user.Claims.Add(new Claim(CustomClaimTypes.Email, user.Email));

            return new AuthenticationResult(user);
        }
    }
    
}