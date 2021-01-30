using System;
using System.Collections.Generic;
using BuyMeIt.Modules.UserAccess.Application.Contracts;

namespace BuyMeIt.Modules.UserAccess.Application.Authentication.Authorization.UserPermissions
{
    public class GetUserPermissionsQuery : QueryBase, IQuery<List<UserPermissionDto>>
    {
        public GetUserPermissionsQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}