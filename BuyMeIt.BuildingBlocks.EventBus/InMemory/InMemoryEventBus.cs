using System;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.EventBus.InMemory
{
    public sealed class InMemoryEventBus
    {
        private readonly List<Subscription> _handlers;

        public IReadOnlyCollection<Subscription> Handlers => _handlers?.AsReadOnly();

        public void ClearHandlers() => _handlers.Clear();
        
        private InMemoryEventBus()
        {
            _handlers = new List<Subscription>();
        }

        public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

        public event EventHandler OnEventPublished;
        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            _handlers.Add(new Subscription(handler, typeof(T).FullName));
        }

        public async Task Publish<T>(T @event) where T : IntegrationEvent
        {
            var eventType = @event.GetType();

            var integrationEventHandlers = _handlers.Where(x => x.EventName == eventType.FullName).ToList();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                if (integrationEventHandler.Handler is IIntegrationEventHandler<T> handler)
                {
                    await handler.Handle(@event);
                    OnEventPublished?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public async Task Publish(string eventName, dynamic @event)
        {
            var integrationEventHandlers = _handlers.Where(x => x.EventName == eventName).ToList();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                await integrationEventHandler.Handler.Handle(@event);
                OnEventPublished?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
