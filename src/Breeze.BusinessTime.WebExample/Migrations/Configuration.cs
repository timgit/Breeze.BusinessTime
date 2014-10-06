using Breeze.BusinessTime.WebExample.Models;
using System.Data.Entity.Migrations;

namespace Breeze.BusinessTime.WebExample.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Dealers.AddOrUpdate(new Dealer {Address = "123 Main", Name = "Cars FTW", Preferred = false});
            context.SaveChanges();
        }
    }
}
