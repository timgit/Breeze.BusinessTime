using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public class RegistryAuthorizationProvider: IAuthorizeAnEntity
    {
        private readonly Dictionary<Type, Authorizer> _registry;

        public RegistryAuthorizationProvider()
        {
            _registry = new Dictionary<Type, Authorizer>();
        }

        public Dictionary<Type, Authorizer> Registry
        {
            get { return _registry; }
        }

        public static RegistryAuthorizationProvider Create()
        {
            return new RegistryAuthorizationProvider();
        }

        public RegistryAuthorizationProvider Register<T>(string roles = null, string users = null)
        {
            _registry.Add(typeof(T), new Authorizer { Roles = roles, Users = users });
            return this;
        }

        public bool IsAuthorized(Type entityType, string userName)
        {
            Authorizer authorizer;

            _registry.TryGetValue(entityType, out authorizer);
            
            return authorizer == null || authorizer.IsAuthorized(userName);
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            Authorizer authorizer;

            _registry.TryGetValue(entityType, out authorizer);

            return authorizer == null || authorizer.IsAuthorized(user);
        }
    }
}
