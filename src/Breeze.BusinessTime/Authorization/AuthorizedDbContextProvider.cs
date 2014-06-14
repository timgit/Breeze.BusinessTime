using System.Data.Entity;
using System.Security.Principal;
using Breeze.BusinessTime.Rules;

namespace Breeze.BusinessTime.Authorization
{
    public class AuthorizedDbContextProvider<T>: PipelinedDbContextProvider<T> where T : DbContext, new()
    {
        public AuthorizedDbContextProvider(IPrincipal user, params string[] allowedRoles)
            : this(user, new AttributeAuthorizationProvider<AuthorizeEntityAttribute>(), allowedRoles) { }

        public AuthorizedDbContextProvider(IPrincipal user, IAuthorizeAnEntity roleProvider, params string[] allowedRoles)
            : base(new AuthorizationProcesser(user, roleProvider, allowedRoles)) { }
    }
}