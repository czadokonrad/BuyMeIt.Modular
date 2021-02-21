using Autofac;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using Microsoft.Extensions.Logging;

namespace BuyMeIt.Modules.UserAccess.Infrastructure.Configuration.EventBus
{
    internal sealed class EventBusStartup : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventsBus, ILogger logger) where T : IntegrationEvent
        {
            logger.LogInformation("Subscribing to {@IntegrationEvent}", typeof(T).FullName);
            eventsBus.Subscribe<T>(new IntegrationEventGenericHandler<T>());
        }
    }
}
