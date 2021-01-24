using BuyMeIt.BuildingBlocks.Domain;
using System;

namespace BuyMeIt.Modules.UserAccess.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}
