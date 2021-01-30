using MediatR;
using System;

namespace BuyMeIt.BuildingBlocks.Infrastructure.EventBus
{
    public abstract class IntegrationEvent : INotification
    {
        public Guid Id { get; }

        public DateTimeOffset OccurredOn { get; }

        
        protected IntegrationEvent(Guid id, DateTimeOffset occurredOn)
        {
            this.Id = id;
            this.OccurredOn = occurredOn;
        }
    }
}
