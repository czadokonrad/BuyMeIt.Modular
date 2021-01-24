using System;

namespace BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainNotificationsMapper
    {
        string GetName(Type type);
        Type GetType(string name);
    }
}
