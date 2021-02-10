using System;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.BuildingBlocks.Infrastructure.Serialization;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Commands;
using BuyMeIt.Modules.UserAccess.Application.Contracts;
using Dapper;
using Newtonsoft.Json;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync(ICommand command)
        {
            var connection = await _sqlConnectionFactory.GetOpenConnectionAsync();

            string insertQuery = @"INSERT INTO [users].[InternalCommands] (Id, EnqueueDate, Type, Data)
                                   VALUES
                                   (@Id, @EnqueueDate, @Type, @Data)";
            
            await connection.ExecuteAsync(insertQuery, new
            {
               command.Id,
               EnqueueDate = DateTimeOffset.UtcNow,
               Type = command.GetType().FullName,
               Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
               {
                   ContractResolver = new AllPropertiesContractResolver()
               })
            });
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            var connection = await _sqlConnectionFactory.GetOpenConnectionAsync();

            string insertQuery = @"INSERT INTO [users].[InternalCommands] (Id, EnqueueDate, Type, Data)
                                   VALUES
                                   (@Id, @EnqueueDate, @Type, @Data)";
            
            await connection.ExecuteAsync(insertQuery, new
            {
                command.Id,
                EnqueueDate = DateTimeOffset.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });
        }
    }
}