using BuyMeIt.BuildingBlocks.Domain;

namespace BuyMeIt.Modules.UserAccess.Domain.Users.Events
{
    public sealed class UserClosedAccountDomainEvent : DomainEventBase
    {
        public UserClosedAccountDomainEvent(UserId id)
        {
            Id = id;
        }

        public new UserId Id { get; }
    }
}
