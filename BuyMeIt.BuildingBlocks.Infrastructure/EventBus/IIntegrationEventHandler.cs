using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure.EventBus
{
    public interface IIntegrationEventHandler
    {
        Task Handle(dynamic @event);
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
