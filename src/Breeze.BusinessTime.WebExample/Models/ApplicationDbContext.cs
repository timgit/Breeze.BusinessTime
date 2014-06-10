using System.Data.Entity;
using Breeze.BusinessTime.WebExample.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Breeze.BusinessTime.WebExample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<IdentityUserRole> UserRoles { get; set; }
    }
}