using System;
using System.Linq;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Surname : ValueObject
    {
        public string Value { get; }

        public Surname(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Any(x => char.IsDigit(x) || (!char.IsLetter(x) && x != '-')) || value.All(c => c == '-'))
                throw new DomainException($"Provided surname: '{value}' is not valid. Contains invalid non-letter characters");

            Value = value;
        }

        public static implicit operator string(Surname firstname) => firstname.Value;

        public static implicit operator Surname(string value) => new Surname(value);

        public override string ToString()
        {
            return Value;
        }
    }
}
