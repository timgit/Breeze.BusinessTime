using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Breeze.ContextProvider;

namespace Breeze.BusinessTime.Authorization
{
    public class AuthorizationProcesser: IProcessBreezeRequests
    {
        private IPrincipal User { get; set; }
        private IEnumerable<string> RoleWhiteList { get; set; }
        private IAuthorizeAnEntity AuthorizationProvider { get; set; }

        public AuthorizationProcesser(IPrincipal user, bool permissive = false, params string[] allowedRoles)
            : this(user, new AttributeAuthorizationProvider<AuthorizeEntityAttribute>(permissive), allowedRoles) { }

        public AuthorizationProcesser(IPrincipal user, IAuthorizeAnEntity authProvider, params string[] allowedRoles)
        {
            if(user == null)
                throw new Exception("IPrincipal cannot be null.");

            if (authProvider == null)
                throw new Exception("IAuthorizeAnEntity cannot be null.");

            User = user;
            AuthorizationProvider = authProvider;
            RoleWhiteList = allowedRoles;
        }

        public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (RoleWhiteList.Any(User.IsInRole)) return;

            saveMap.ToList().ForEach(map =>
            {
                var entityType = map.Key;

                if (!AuthorizationProvider.IsAuthorized(entityType, User))
                {
                    throw new EntityErrorsException(new[]{
                        new EntityError{ 
                            EntityTypeName = entityType.ToString(),
                            ErrorName = "Unauthorized",
                            ErrorMessage = "You are not authorized to save changes to this entity."
                        }});
                }
            });
        }
    }
}