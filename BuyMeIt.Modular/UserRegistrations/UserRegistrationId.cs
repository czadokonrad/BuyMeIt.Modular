using BuyMeIt.BuildingBlocks.Domain;
using System;

namespace BuyMeIt.Modules.UserAccess.Domain.UserRegistrations
{
    public class UserRegistrationId : TypedIdValueBase
    {
        public UserRegistrationId(Guid value)
            : base(value)
        {
        }
    }
}
