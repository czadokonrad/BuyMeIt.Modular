using System;
using System.ComponentModel.DataAnnotations;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public static EmailAddressAttribute _validator = new EmailAddressAttribute();
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (!_validator.IsValid(value))
                throw new ArgumentException(nameof(value));

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
