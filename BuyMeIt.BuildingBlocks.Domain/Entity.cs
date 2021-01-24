using System;
using System.Collections.Generic;

namespace BuyMeIt.BuildingBlocks.Domain
{
#nullable enable
    public abstract class Entity
    {
        private List<IDomainEvent>? _domainEvents;

        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents() => 
            _domainEvents?.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            _domainEvents ??= new List<IDomainEvent>();

            _domainEvents.Add(domainEvent);
        }

        protected void RemoveDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents?.Remove(domainEvent);

        protected void CheckRule(IBusinessRule rule)
        {
            if(rule.IsBroken())
            {
                throw new BusinessRuleViolationException(rule);
            }
        }
    }
#nullable restore
}
