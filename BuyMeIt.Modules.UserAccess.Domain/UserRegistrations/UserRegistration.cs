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

        public string Login { get; }

        public string Password { get; }

        public string Email { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Name { get; }

        public DateTimeOffset RegisterDate { get; }

        public UserRegistrationStatus Status { get; private set; }

        public DateTimeOffset? ConfirmedDate { get; private set; }

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
            Login = login;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Name = $"{firstName} {lastName}";
            RegisterDate = DateTimeOffset.UtcNow;
            Status = UserRegistrationStatus.WaitingForConfirmation;

            this.AddDomainEvent(new NewUserRegisteredDomainEvent(
                this.Id,
                Login,
                Email,
                FirstName,
                LastName,
                Name,
                RegisterDate,
                confirmLink));
        }

        public User CreateUser()
        {
            this.CheckRule(new UserCannotBeCreatedWhenRegistrationIsNotConfirmedRule(Status));

            return User.CreateFromUserRegistration(
                this.Id,
                this.Login,
                this.Password,
                this.Email,
                this.FirstName,
                this.LastName,
                this.Name);
        }

        public void Confirm()
        {
            this.CheckRule(new UserRegistrationCannotBeConfirmedMoreThanOnceRule(Status));
            this.CheckRule(new UserRegistrationCannotBeConfirmedAfterExpirationRule(Status));

            Status = UserRegistrationStatus.Confirmed;
            ConfirmedDate = DateTimeOffset.UtcNow;

            this.AddDomainEvent(new UserRegistrationConfirmedDomainEvent(this.Id));
        }

        public void Expire()
        {
            this.CheckRule(new UserRegistrationCannotBeExpiredMoreThanOnceRule(Status));

            Status = UserRegistrationStatus.Expired;

            this.AddDomainEvent(new UserRegistrationExpiredDomainEvent(this.Id));
        }
    }
}
