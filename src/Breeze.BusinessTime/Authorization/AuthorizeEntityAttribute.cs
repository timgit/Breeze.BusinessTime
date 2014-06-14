using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeEntityAttribute: Attribute, IAuthorize
    {
        public string Roles
        {
            get { return _authorizer.Roles; }
            set { _authorizer.Roles = value; }
        }

        public string Users
        {
            get { return _authorizer.Users; }
            set { _authorizer.Users = value; }
        }

        private readonly Authorizer _authorizer = new Authorizer();

        public bool IsAuthorized()
        {
            return _authorizer.IsAuthorized();
        }

        public bool IsAuthorized(string userName)
        {
            return _authorizer.IsAuthorized(userName);
        }

        public bool IsAuthorized(IPrincipal user)
        {
            return _authorizer.IsAuthorized(user);
        }

        public IEnumerable<string> GetUsers()
        {
            return _authorizer.GetUsers();
        }

        public IEnumerable<string> GetRoles()
        {
            return _authorizer.GetRoles();
        }
    }
}