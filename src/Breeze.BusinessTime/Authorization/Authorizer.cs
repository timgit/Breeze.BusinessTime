using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public class Authorizer: IAuthorize
    {
        public string Users { get; set; }
        public string Roles { get; set; }

        /// <summary>
        /// An empty IsAuthorized call would be for checking if a user is authenticated or not
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorized()
        {
            return (Users == null && Roles == null);
        }

        public bool IsAuthorized(string userName)
        {
            return HasUser(userName);
        }

        public bool IsAuthorized(IPrincipal user)
        {
            return HasUser(user.Identity.Name) || HasRole(user);
        }

        private bool HasRole(IPrincipal user)
        {
            return Roles != null && GetRoles().Any(user.IsInRole);
        }

        private bool HasUser(string user)
        {
            return Users != null && GetUsers().Any(u => u.Equals(user, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<string> GetUsers()
        {
            return Users.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public IEnumerable<string> GetRoles()
        {
            return Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
