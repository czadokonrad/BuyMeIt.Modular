using System;
using System.Text.RegularExpressions;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Name : ValueObject
    {
        public static Regex _validator = new Regex("^[a-zA-Z]+$");

        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (!_validator.IsMatch(value))
                throw new ArgumentException(nameof(value));

            Value = value;
        }

        public static implicit operator string(Name firstname) => firstname.Value;

        public static implicit operator Name(string value) => new Name(value);

        public override string ToString()
        {
            return Value;
        }
    }
}
