﻿using System;
using System.Threading.Tasks;
using BuyMeIt.BuildingBlocks.Infrastructure.EventBus;

namespace BuyMeIt.BuildingBlocks.EventBus.Tests
{
    
    public class EventBusTestsBase
    {
        public class FirstTestEvent : IntegrationEvent
        {
            public FirstTestEvent(Guid id, DateTimeOffset occurredOn) : base(id, occurredOn)
            {
            }
        }

        public class FirstTestEventHandler : IIntegrationEventHandler<FirstTestEvent>
        {
            public Task Handle(FirstTestEvent @event)
            {
                return Task.CompletedTask;
            }

            public Task Handle(dynamic @event)
            {
                return Task.CompletedTask;
            }
        }
        
        public class SecondTestEvent : IntegrationEvent
        {
            public SecondTestEvent(Guid id, DateTimeOffset occurredOn) : base(id, occurredOn)
            {
            }
        }

        public class SecondTestEventHandler : IIntegrationEventHandler<SecondTestEvent>
        {
            public Task Handle(SecondTestEvent @event)
            {
                return Task.CompletedTask;
            }
            
            public Task Handle(dynamic @event)
            {
                return Task.CompletedTask;
            }
        }
        
        public class ThirdTestEvent : IntegrationEvent
        {
            public ThirdTestEvent(Guid id, DateTimeOffset occurredOn) : base(id, occurredOn)
            {
            }
        }

        public class ThirdTestEventHandler : IIntegrationEventHandler<ThirdTestEvent>
        {
            public Task Handle(ThirdTestEvent @event)
            {
                return Task.CompletedTask;
            } 
            public Task Handle(dynamic @event)
            {
                return Task.CompletedTask;
            }
        }

        public class FourthTestEvent : IntegrationEvent
        {
            public FourthTestEvent(Guid id, DateTimeOffset occurredOn) : base(id, occurredOn)
            {
            }
        }

        public class FourthTestEventHandler : IIntegrationEventHandler<FourthTestEvent>
        {
            public Task Handle(FourthTestEvent @event)
            {
                return Task.CompletedTask;
            }
            public Task Handle(dynamic @event)
            {
                return Task.CompletedTask;
            }
        }
    }
}