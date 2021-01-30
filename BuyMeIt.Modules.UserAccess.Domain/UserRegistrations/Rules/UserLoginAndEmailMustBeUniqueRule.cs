using BuyMeIt.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations.Rules
{
#nullable enable
    public sealed class UserLoginAndEmailMustBeUniqueRule : IBusinessRule
    {
        private readonly IUserUniqueness _userUniqueness;
        private readonly string _login;
        private readonly string _email;

        internal UserLoginAndEmailMustBeUniqueRule(IUserUniqueness userUniqueness, string login, string email)
        {
            _userUniqueness = userUniqueness;
            _login = login;
            _email = email;
        }

        public bool IsBroken() => !_userUniqueness.IsUserUnique(_login, _email);

        public string Message => "User Login and Email must be unique";
    }

#nullable restore
}
