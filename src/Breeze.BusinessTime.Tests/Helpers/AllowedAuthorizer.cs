using System;
using System.Security.Principal;
using Breeze.BusinessTime.Authorization;

namespace Breeze.BusinessTime.Tests.Helpers
{
    public class AllowedAuthorizer: IAuthorizeAnEntity
    {
        public bool IsAuthorized(Type entityType, string userName)
        {
            return true;
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            return true;
        }
    }
}
