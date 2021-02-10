using System;
using BuyMeIt.BuildingBlocks.Domain.Common.ValueObjects;
using NUnit.Framework;

namespace BuyMeIt.BuildingBlocks.Domain.Tests.Common
{
    [TestFixture]
    public class CommonValueObjectsTests
    {
        #region EmailValueObject

        [Test]
        public void EmailValueObject_WhenEmptyStringPassedInConstructor_Should_Throw_DomainException()
        {
            var ex = Assert.Throws<DomainException>(() => new Email(string.Empty));
            Assert.That(ex.Message, Is.EqualTo($"Provided email: '' is not an valid email address"));
        }
        
        [Test]
        public void EmailValueObject_WhenEmptyNullPassedInConstructor_Should_Throw_ArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Email(null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'value')"));
        }
        
        [Test]
        public void EmailValueObject_WhenValueIsInvalidEmail_Should_Throw_DomainException()
        {
            string invalidEmail = "someinvalidemail";
            var ex = Assert.Throws<DomainException>(() => new Email(invalidEmail));
            Assert.That(ex.Message, Is.EqualTo($"Provided email: '{invalidEmail}' is not an valid email address"));
        }

        [Test]
        public void EmailValueObject_WhenValueIsValidEmail_Should_CreateObject()
        {
            string email = "test@gmail.com";

            var emailValue = new Email(email);
            
            Assert.That(email, Is.EqualTo(emailValue.Value));
        }
        
        [Test]
        public void EmailValueObject_WhenValueIsValidEmail_Should_CreateObject_AndShouldSuccesfullyImlicitlyConvert_ToString()
        {
            string email = "test@gmail.com";

            var emailValue = new Email(email);

            string emailStringValue = emailValue;
            
            Assert.That(emailStringValue, Is.EqualTo(email));
        }
        
        #endregion

        #region FullnameValueObject

        [Test]
        public void FullnameValueObject_When_MultipleNamesPassed_AndSurname_ShouldCreateObject_And_HasProperString_InValueProperty()
        {
            string[] names =
            {
                "Michael",
                "Steven",
                "Jack"
            };

            string surname = "Jordan";

            var fullName = new Fullname(names, surname);
            
            string expected = $"{string.Join(" ", names)} {surname}";
            
            Assert.That(fullName.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void FullnameValueObject_When_AnyNameIsInvalid_ShouldThrowDomainException_With_ErrorMessage()
        {
            string[] names =
            {
                "654654645654",
                "Steven",
                "Jack"
            };

            string surname = "Pudzianowski";

            var ex = Assert.Throws<DomainException>(() => new Fullname(names, surname));
            
            Assert.AreEqual(ex.Message, $"Provided name: '{names[0]}' is not valid. Contains invalid non-letter characters");
            
        }

        #endregion
    }
}