using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public class RegistryAuthorizationProvider: IAuthorizeAnEntity
    {
        private readonly bool _permissive;
        private readonly Dictionary<Type, Authorizer> _registry;

        public RegistryAuthorizationProvider(bool permissive = false)
        {
            _permissive = permissive;
            _registry = new Dictionary<Type, Authorizer>();
        }

        public Dictionary<Type, Authorizer> Registry
        {
            get { return _registry; }
        }

        public static RegistryAuthorizationProvider Create(bool permissive = false)
        {
            return new RegistryAuthorizationProvider(permissive);
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

            //TODO: Refactor this into common permissive behavior layer so tests can be consolidated with other auth providers
            return authorizer != null ? authorizer.IsAuthorized(userName) : _permissive;
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            Authorizer authorizer;

            _registry.TryGetValue(entityType, out authorizer);

            //TODO: Refactor this into common permissive behavior layer so tests can be consolidated
            return authorizer != null ? authorizer.IsAuthorized(user) : _permissive;
        }
    }
}
