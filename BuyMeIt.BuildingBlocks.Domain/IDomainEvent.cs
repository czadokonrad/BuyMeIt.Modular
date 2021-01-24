using MediatR;
using System;

namespace BuyMeIt.BuildingBlocks.Domain
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTimeOffset OccurredOn { get; }
    }
}
