using System;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public interface IAuthorizeAnEntity
    {
        bool IsAuthorized(Type entityType, string userName);
        bool IsAuthorized(Type entityType, IPrincipal user);
    }
}