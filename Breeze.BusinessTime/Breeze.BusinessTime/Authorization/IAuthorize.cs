using System.Collections.Generic;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public interface IAuthorize
    {
        IEnumerable<string> GetRoles();
        IEnumerable<string> GetUsers();
        bool IsAuthorized(string userName);
        bool IsAuthorized(IPrincipal user);
    }
}