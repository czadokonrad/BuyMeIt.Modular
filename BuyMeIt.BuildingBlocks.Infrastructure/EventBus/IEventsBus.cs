using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventsBus
    {
        Task Publish<T>(T @event) where T : IntegrationEvent;

        void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;
    }
}
