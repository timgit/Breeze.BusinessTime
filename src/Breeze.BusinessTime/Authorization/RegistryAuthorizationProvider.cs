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

        public static RegistryAuthorizationProvider Create()
        {
            return new RegistryAuthorizationProvider();
        }

        public RegistryAuthorizationProvider Register<T>(string roles = null, string users = null)
        {
            return Register(typeof(T), roles, users);
        }

        public RegistryAuthorizationProvider Register(Type type, string roles = null, string users = null)
        {
            if (_registry.ContainsKey(type))
                throw new ArgumentException("This entity type is already in the registry.");

            _registry.Add(type, new Authorizer
            {
                Roles = roles,
                Users = users
            });

            return this;
        }

        public bool IsAuthorized(Type entityType, string userName)
        {
            Authorizer authorizer;

            _registry.TryGetValue(entityType, out authorizer);
            
            return authorizer != null && authorizer.IsAuthorized(userName);
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            Authorizer authorizer;

            _registry.TryGetValue(entityType, out authorizer);

            return authorizer != null && authorizer.IsAuthorized(user);
        }
    }
}
