using System;
using System.Linq;

namespace BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects
{
    public sealed class Fullname : ValueObject
    {
        public Name[] Names { get; }
        public Surname Surname { get; }

        public Fullname(string[] names, string surname)
        {
            Names = new Name[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                Names[i] = new Name(names[i]);
            }

            Surname = new Surname(surname);
        }

        public Fullname(Name[] names, Surname surname)
        {
            if(names == null || names.Length == 0)
            {
                throw new ArgumentException(nameof(names));
            }

            Names = names;
            Surname = surname;
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Names.Select(n => n.Value))} {Surname.Value}";
        }
    }
}
