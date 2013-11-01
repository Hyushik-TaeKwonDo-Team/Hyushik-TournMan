namespace Hyushik_TournMan_DAL.Migrations
{
    using Hyushik_TournMan_Common.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Hyushik_TournMan_DAL.Contexts.TournManContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Hyushik_TournMan_DAL.Contexts.TournManContext context)
        {
            //  This method will be called after migrating to the latest version.

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


            context.Tournaments.AddOrUpdate(
                t=> t.Name,
                new Tournament(){Name="Tournament 1"},
                new Tournament(){Name="Tournament 2"}
                );
        }
    }
}
