using Microsoft.AspNet.Identity.EntityFramework;

namespace Breeze.BusinessTime.WebExample.Services.Identity
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole() {}
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}