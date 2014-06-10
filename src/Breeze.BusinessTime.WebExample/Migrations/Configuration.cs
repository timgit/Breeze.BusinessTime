using System.Data.Entity.Migrations;

namespace Breeze.BusinessTime.WebExample.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Breeze.BusinessTime.WebExample.Models.ApplicationDbContext";
        }

        protected override void Seed(Breeze.BusinessTime.WebExample.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            // add roles

            // add admin user
            // add dealer user
            // add owner user

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
