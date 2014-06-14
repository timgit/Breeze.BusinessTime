using System.Collections.Generic;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public interface IAuthorize
    {
        string Users { get; set; }
        string Roles { get; set; }
        IEnumerable<string> GetRoles();
        IEnumerable<string> GetUsers();
        bool IsAuthorized();
        bool IsAuthorized(string userName);
        bool IsAuthorized(IPrincipal user);
    }
}