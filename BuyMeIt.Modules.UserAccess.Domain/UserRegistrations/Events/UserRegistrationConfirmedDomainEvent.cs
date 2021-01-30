using BuyMeIt.BuildingBlocks.Domain;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Events
{
    public sealed class UserRegistrationConfirmedDomainEvent : DomainEventBase
    {
        public UserRegistrationConfirmedDomainEvent(UserRegistrationId userRegistrationId)
        {
            UserRegistrationId = userRegistrationId;
        }

        public UserRegistrationId UserRegistrationId { get; }
    }
}
