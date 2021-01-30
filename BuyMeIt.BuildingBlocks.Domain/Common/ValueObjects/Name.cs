using System;
using System.Linq;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Name : ValueObject
    {
        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (value.Any(x => char.IsLetter(x)))
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
