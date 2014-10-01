using System.Data.Entity.Migrations;
using Breeze.BusinessTime.WebExample.Models;
using Breeze.BusinessTime.WebExample.Services.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Breeze.BusinessTime.WebExample.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Breeze.BusinessTime.WebExample.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            roleManager.CreateRole("Admin");
            roleManager.CreateRole("Dealer");
            roleManager.CreateRole("Owner");

            userManager.CreateUser("andy@admin.com", "999999", "Admin");
            userManager.CreateUser("don@dealer.com", "333333", "Dealer");
            userManager.CreateUser("bob@owner.com", "111111", "Owner");
        }
    }

    public static class Extensions
    {
        public static void CreateRole(this ApplicationRoleManager roleManager, string name)
        {
            if (!roleManager.RoleExists(name))
                roleManager.Create(new ApplicationRole(name));
        }

        public static void CreateUser(this ApplicationUserManager userManager, string username, string password, string role)
        {
            if (userManager.FindByName(username) != null)
                return;

            var user = new ApplicationUser
            {
                UserName = username, 
                Email = username, 
                EmailConfirmed = true
            };

            var result = userManager.Create(user, password);

            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, role);
            }
        }
    }
}
