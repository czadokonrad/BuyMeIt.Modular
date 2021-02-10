using System;
using System.Linq;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Name : ValueObject
    {
        public string Value { get; }

        public Name(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Any(x => !char.IsLetter(x)))
                throw new DomainException($"Provided name: '{value}' is not valid. Contains invalid non-letter characters");

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
