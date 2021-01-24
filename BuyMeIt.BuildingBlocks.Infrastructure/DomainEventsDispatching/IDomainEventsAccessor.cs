using BuyMeIt.BuildingBlocks.Domain;
using System.Collections.Generic;

namespace BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}
