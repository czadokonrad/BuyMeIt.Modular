﻿using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.EventBus.InMemory
{
    public sealed class InMemoryEventBus
    {
        private readonly List<Subscription> _handlers;

        private InMemoryEventBus()
        {
            _handlers = new List<Subscription>();
        }

        public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            _handlers.Add(new Subscription(handler, typeof(T).FullName));
        }

        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            var eventType = @event.GetType();

            var integrationEventHandlers = _handlers.Where(x => x.EventName == eventType.FullName).ToList();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                if (integrationEventHandler.Handler is IIntegrationEventHandler<T> handler)
                {
                    await handler.Handle(@event);
                }
            }
        }
    }
}