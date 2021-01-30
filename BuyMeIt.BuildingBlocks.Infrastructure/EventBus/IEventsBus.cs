using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure.EventBus
{
    public interface IEventsBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IntegrationEvent;

        void Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : IntegrationEvent;
    }
}
