using Autofac;
using Autofac.Core;
using BuyMeIt.BuildingBlocks.Application.DomainEvents;
using BuyMeIt.BuildingBlocks.Application.Outbox;
using BuyMeIt.BuildingBlocks.Domain;
using BuyMeIt.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public sealed class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _scope;
        private readonly IOutbox _outbox;
        private readonly IDomainEventsAccessor _domainEventsAccessor;
        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        public DomainEventsDispatcher(
            IMediator mediator,
            ILifetimeScope scope,
            IOutbox outbox,
            IDomainEventsAccessor domainEventsAccessor,
            IDomainNotificationsMapper domainNotificationsMapper)
        {
            this._mediator = mediator;
            this._scope = scope;
            this._outbox = outbox;
            this._domainEventsAccessor = domainEventsAccessor;
            this._domainNotificationsMapper = domainNotificationsMapper;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();

            foreach(var domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", domainEvent.Id)
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            _domainEventsAccessor.ClearAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            foreach (var domainEventNotification in domainEventNotifications)
            {
                var type = _domainNotificationsMapper.GetName(domainEventNotification.GetType());
                var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });

                var outboxMessage = new OutboxMessage(
                    domainEventNotification.Id,
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);

                _outbox.Add(outboxMessage);
            }
        }
    }
}
