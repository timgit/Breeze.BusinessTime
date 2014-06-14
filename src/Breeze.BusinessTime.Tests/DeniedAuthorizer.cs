using System;
using System.Security.Principal;
using Breeze.BusinessTime.Authorization;

namespace Breeze.BusinessTime.Tests
{
    public class DeniedAuthorizer: IAuthorizeAnEntity
    {
        public bool IsAuthorized(Type entityType, string userName)
        {
            return false;
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            return false;
        }
    }
}
