using System.Security.Principal;

namespace Breeze.BusinessTime.Authorization
{
    public class AuthorizedDbContextProvider<T>: PipelinedDbContextProvider<T> where T : class, new()
    {
        public AuthorizedDbContextProvider(IPrincipal user, bool permissive, params string[] allowedRoles)
            : this(user, new AttributeAuthorizationProvider<AuthorizeEntityAttribute>(permissive), allowedRoles) { }

        public AuthorizedDbContextProvider(IPrincipal user, params string[] allowedRoles)
            : this(user, new AttributeAuthorizationProvider<AuthorizeEntityAttribute>(permissive: false), allowedRoles) { }

        public AuthorizedDbContextProvider(IPrincipal user, IAuthorizeAnEntity roleProvider, params string[] allowedRoles)
            : base(new AuthorizationProcesser(user, roleProvider, allowedRoles)) { }
    }
}