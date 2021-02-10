using System;
using System.ComponentModel.DataAnnotations;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly EmailAddressAttribute Validator = new EmailAddressAttribute();
        public string Value { get; }

        public Email(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!Validator.IsValid(value))
                throw new DomainException($"Provided email: '{value}' is not an valid email address");

            Value = value;
        }

        public static implicit operator string(Email email) => email.Value;

        public static implicit operator Email(string value) => new Email(value);

        public override string ToString()
        {
            return Value;
        }
    }
}
