using BuyMeIt.BuildingBlocks.Domain;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Rules
{
    public sealed class UserRegistrationCannotBeConfirmedMoreThanOnceRule : IBusinessRule
    {
        private readonly UserRegistrationStatus _actualRegistrationStatus;

        internal UserRegistrationCannotBeConfirmedMoreThanOnceRule(UserRegistrationStatus actualRegistrationStatus)
        {
            this._actualRegistrationStatus = actualRegistrationStatus;
        }

        public bool IsBroken() => _actualRegistrationStatus == UserRegistrationStatus.Confirmed;

        public string Message => "User Registration cannot be confirmed more than once";
    }
}
