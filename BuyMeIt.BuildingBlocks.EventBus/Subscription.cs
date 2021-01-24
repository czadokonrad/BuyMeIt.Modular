using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;

namespace BuyMeIt.BuildingBlocks.EventBus
{
    public struct Subscription
    {
        public Subscription(IIntegrationEventHandler handler, string eventName)
        {
            Handler = handler;
            EventName = eventName;
        }

        public IIntegrationEventHandler Handler { get; }

        public string EventName { get; }

    }
}
