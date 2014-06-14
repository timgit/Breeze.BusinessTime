using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Principal;
using Breeze.BusinessTime.Rules;

namespace Breeze.BusinessTime.Authorization
{
    public class AuthorizedDbContextProvider<T>: PipelinedDbContextProvider<T> where T : DbContext, new()
    {
        public AuthorizedDbContextProvider(IPrincipal user)
            : this(user, new AttributeAuthorizationProvider<AuthorizeEntityAttribute>(), new string[] { }) { }

        public AuthorizedDbContextProvider(IPrincipal user, IAuthorizeAnEntity roleProvider) 
            : this(user, roleProvider, new string[] { }) { }

        public AuthorizedDbContextProvider(IPrincipal user, IAuthorizeAnEntity roleProvider, string allowedRole) 
            : this(user, roleProvider, new[] { allowedRole }) { }

        public AuthorizedDbContextProvider(IPrincipal user, IAuthorizeAnEntity roleProvider, IEnumerable<string> allowedRoles)
            : base(new AuthorizationProcesser(user, roleProvider, allowedRoles)) { }
    }
}