using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Breeze.BusinessTime.Rules;
using Breeze.ContextProvider;

namespace Breeze.BusinessTime.Authorization
{
    public class AuthorizationProcesser: IProcessBreezeRequests
    {
        private IPrincipal User { get; set; }
        private IEnumerable<string> RoleWhiteList { get; set; }
        private IAuthorizeAnEntity RoleProvider { get; set; }

        public AuthorizationProcesser(IPrincipal user, IAuthorizeAnEntity roleProvider) : this(user, roleProvider, new string[] { }) { }

        public AuthorizationProcesser(IPrincipal user, IAuthorizeAnEntity roleProvider, string allowedRole) : this(user, roleProvider, new[] { allowedRole }) { }

        public AuthorizationProcesser(IPrincipal user, IAuthorizeAnEntity roleProvider, IEnumerable<string> allowedRoles)
        {
            if(user == null)
                throw new Exception("IPrincipal cannot be null.");

            if (roleProvider == null)
                throw new Exception("IProvideRolesForAnEntity cannot be null.");

            User = user;
            RoleProvider = roleProvider;            
            RoleWhiteList = allowedRoles ?? new string[] {};      
        }

        public void Process(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (RoleWhiteList.Any(User.IsInRole)) return;

            saveMap.ToList().ForEach(map =>
            {
                var entityType = map.Key;

                if (!RoleProvider.IsAuthorized(entityType, User))
                {
                    var errors = map.Value.Select(oi =>
                        new EntityError
                        {
                            EntityTypeName = oi.Entity.ToString(),
                            ErrorName = "Unauthorized",
                            ErrorMessage = "You are not authorized to save changes to this entity."
                        });

                    throw new EntityErrorsException(errors);
                }
            });
        }
    }
}