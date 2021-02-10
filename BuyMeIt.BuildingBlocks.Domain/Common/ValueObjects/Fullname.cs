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
            if (names == null)
                throw new ArgumentNullException(nameof(names));
            if (surname == null)
                throw new ArgumentNullException(nameof(surname));
            
            if(names.Length == 0)
            {
                throw new DomainException("At least one one has to be provided");
            }
            
            Names = new Name[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                Names[i] = new Name(names[i]);
            }

            Surname = new Surname(surname);
        }

        public Fullname(Name[] names, Surname surname)
        {
            if (names == null)
                throw new ArgumentNullException(nameof(names));
            if (surname == null)
                throw new ArgumentNullException(nameof(surname));
            
            if(names.Length == 0)
            {
                throw new DomainException("At least one one has to be provided");
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
