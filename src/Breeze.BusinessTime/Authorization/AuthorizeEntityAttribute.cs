using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeEntityAttribute: Attribute, IAuthorize
    {
        public string Roles { get; set; }
        public string Users { get; set; }

        public bool IsAuthorized(string userName)
        {
            return HasUser(userName);
        }

        public bool IsAuthorized(IPrincipal user)
        {
            return HasUser(user.Identity.Name) || GetRoles().Any(user.IsInRole);
        }

        private bool HasUser(string user)
        {
            return GetUsers().Any(u => u.Equals(user));
        }

        public IEnumerable<string> GetUsers()
        {
            return Users.Split(',');
        }

        public IEnumerable<string> GetRoles()
        {
            return Roles.Split(',');
        }
    }
}