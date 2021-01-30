using BuyMeIt.BuildingBlocks.Application.Data;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using BuyMeIt.BuildingBlocks.Infrastructure.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Autofac;
using Dapper;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal  sealed class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        public async Task Handle(T @event)
        {
            using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
            {
                using (var connection = await scope.Resolve<ISqlConnectionFactory>().GetOpenConnectionAsync())
                {
                    string type = @event.GetType().FullName;
                    var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                    {
                        ContractResolver = new AllPropertiesContractResolver()
                    });

                    var sql = "INSERT INTO [users].[InboxMessages] (Id, OccurredOn, Type, Data) " +
                              "VALUES (@Id, @OccurredOn, @Type, @Data)";

                    await connection.ExecuteScalarAsync(sql, new
                    {
                        @event.Id,
                        @event.OccurredOn,
                        type,
                        data
                    });
                }
            }
        }
    }
}
