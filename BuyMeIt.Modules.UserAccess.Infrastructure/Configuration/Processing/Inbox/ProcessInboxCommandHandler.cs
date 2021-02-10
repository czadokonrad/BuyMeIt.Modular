using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.Modules.UserAccess.Application.Configuration.Commands;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    public class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ILogger _logger;

        public ProcessInboxCommandHandler(
            IMediator mediator, 
            ISqlConnectionFactory sqlConnectionFactory,
            ILogger logger)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
        {
            var connection = await _sqlConnectionFactory.GetOpenConnectionAsync();

            string sql = $@"SELECT [InboxMessage].[Id] AS [{nameof(InboxMessageDto.Id)}], 
                            [InboxMessage].[Type] AS [{nameof(InboxMessageDto.Type)}],
                            [InboxMessage].[Data] AS [{nameof(InboxMessageDto.Data)}] 
                            FROM [users].[InboxMessages] AS [InboxMessage] 
                            WHERE [InboxMessage].[ProcessedDate] IS NULL 
                            ORDER BY [InboxMessage].[OccurredOn]";

            var messages = await connection.QueryAsync<InboxMessageDto>(sql);
            
            const string sqlUpdateProcessedDate = @"UPDATE [users].[InboxMessages] 
                                                  SET [ProcessedDate] = @Date 
                                                  WHERE [Id] = @Id";

            foreach (var message in messages)
            {
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));

                if (messageAssembly == null) continue;
                
                var type = messageAssembly.GetType(message.Type);
                var request = JsonConvert.DeserializeObject(message.Data, type);
                
                try
                {
                    await _mediator.Publish((INotification)request, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error in ProcessInboxCommandHandler");
                    throw;
                }

                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTimeOffset.UtcNow,
                    Id = message.Id
                });
            }

            return Unit.Value;
        }
    }
}