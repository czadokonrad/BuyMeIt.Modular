using BuyMeIt.BuildingBlocks.Domain;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public sealed class UserRegistrationExpiredDomainEvent : DomainEventBase
    {
        public UserRegistrationExpiredDomainEvent(UserRegistrationId userRegistrationId)
        {
            UserRegistrationId = userRegistrationId;
        }

        public UserRegistrationId UserRegistrationId { get; }
    }
}
