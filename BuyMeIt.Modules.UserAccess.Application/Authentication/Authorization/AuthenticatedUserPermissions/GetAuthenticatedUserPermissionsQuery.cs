using System.Collections.Generic;
using BuyMeIt.Modules.UserAccess.Application.Authentication.Authorization.UserPermissions;
using BuyMeIt.Modules.UserAccess.Application.Contracts;

namespace BuyMeIt.Modules.UserAccess.Application.Authentication.Authorization.AuthenticatedUserPermissions
{
    public class GetAuthenticatedUserPermissionsQuery : QueryBase, IQuery<List<UserPermissionDto>>
    {
    }
}