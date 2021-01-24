using BuyMeIt.BuildingBlocks.Domain;
using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Events;
using BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Rules;
using BuyMeIt.Modules.UserAccess.Domain.Users;
using System;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistration : Entity, IAggregateRoot
    {
        public UserRegistrationId Id { get; private set; }

        private string _login;

        private string _password;

        private string _email;

        private string _firstName;

        private string _lastName;

        private string _name;

        private DateTimeOffset _registerDate;

        private UserRegistrationStatus _status;

        private DateTimeOffset? _confirmedDate;

        private UserRegistration()
        {
            // Only EF.
        }

        public static UserRegistration RegisterNewUser(
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            IUserUniqueness userUniqueness,
            string confirmLink)
        {
            return new UserRegistration(login, password, email, firstName, lastName, userUniqueness, confirmLink);
        }

        private UserRegistration(
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            IUserUniqueness userUniqueness,
            string confirmLink)
        {
            this.CheckRule(new UserLoginAndEmailMustBeUniqueRule(userUniqueness, login, email));

            this.Id = new UserRegistrationId(Guid.NewGuid());
            _login = login;
            _password = password;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = $"{firstName} {lastName}";
            _registerDate = DateTimeOffset.UtcNow;
            _status = UserRegistrationStatus.WaitingForConfirmation;

            this.AddDomainEvent(new NewUserRegisteredDomainEvent(
                this.Id,
                _login,
                _email,
                _firstName,
                _lastName,
                _name,
                _registerDate,
                confirmLink));
        }

        public User CreateUser()
        {
            this.CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(_status));

            return User.CreateFromUserRegistration(
                this.Id,
                this._login,
                this._password,
                this._email,
                this._firstName,
                this._lastName,
                this._name);
        }

        public void Confirm()
        {
            this.CheckRule(new UserRegistrationCannotBeConfirmedMoreThanOnceRule(_status));
            this.CheckRule(new UserRegistrationCannotBeConfirmedAfterExpirationRule(_status));

            _status = UserRegistrationStatus.Confirmed;
            _confirmedDate = DateTimeOffset.UtcNow;

            this.AddDomainEvent(new UserRegistrationConfirmedDomainEvent(this.Id));
        }

        public void Expire()
        {
            this.CheckRule(new UserRegistrationCannotBeExpiredMoreThanOnceRule(_status));

            _status = UserRegistrationStatus.Expired;

            this.AddDomainEvent(new UserRegistrationExpiredDomainEvent(this.Id));
        }
    }
}
