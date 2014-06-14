using System.Security.Principal;

namespace Breeze.BusinessTime.Tests.Helpers
{
    public static class PrincipalHelper
    {
        private const string AdminRoleName = "Admin";

        public static IPrincipal CreatePrincipal(string userName = "", params string[] roles)
        {
            return new GenericPrincipal(new GenericIdentity(userName), roles);
        }

        public static IPrincipal CreateAdminPrincipal(string userName = AdminRoleName)
        {
            return CreatePrincipal(userName, AdminRoleName);
        }

        public static string GetAdminRoleName()
        {
            return AdminRoleName;
        }
    }
}
