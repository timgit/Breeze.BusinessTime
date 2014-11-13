using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public class AttributeAuthorizationProvider<T> : IAuthorizeAnEntity where T : class, IAuthorize
    {
        private readonly bool _permissive;

        public AttributeAuthorizationProvider(bool permissive)
        {
            //TODO: Add tests for permissive flag
            _permissive = permissive;
        }

        public IEnumerable<string> GetAuthorizedRolesForEntity(Type entityType)
        {
            return GetAttribute(entityType).GetRoles();
        }

        public IEnumerable<string> GetAuthorizedUsersForEntity(Type entityType)
        {
            return GetAttribute(entityType).GetUsers();
        }

        public bool IsAuthorized(Type entityType, string userName)
        {
            var attribute = GetAttribute(entityType);

            return attribute != null ? attribute.IsAuthorized(userName) : _permissive;
        }

        public bool IsAuthorized(Type entityType, IPrincipal user)
        {
            var attribute = GetAttribute(entityType);

            return attribute != null ? attribute.IsAuthorized(user) : _permissive;
        }

        private static T GetAttribute(Type entityType)
        {
            var attribute = entityType.GetCustomAttributes(true)
                .FirstOrDefault(a => a is T);

            return attribute != null ? (T)attribute : null;
        }
    }
}