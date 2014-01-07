namespace Hyushik_TournMan_DAL.Migrations
{
    using Hyushik_TournMan_Common.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class MainConfiguration : DbMigrationsConfiguration<Hyushik_TournMan_DAL.Contexts.TournManContext>
    {
        public MainConfiguration()
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
                new Tournament(){Name="Tournament 1", Active=true},
                new Tournament(){Name="Tournament 2"}
                );

            var toggleTech = new Technique()
            {
                Name = "Toggle Tech",
                Toggleable = true
            };

            var parentTech = new Technique()
            {
                Name = "Parent Tech",
                Toggleable = false
            };

            var childTech = new Technique()
            {
                Name = "Child Tech",
                Toggleable = false
            };

            var secondaryChildTech = new Technique()
            {
                Name = "Secondary Child Tech",
                Toggleable = false
            };

            parentTech.AddSubTechnique(childTech);
            childTech.AddSubTechnique(secondaryChildTech);

            context.Techniques.AddOrUpdate(
                t=>t.Name,
                toggleTech,parentTech,childTech, secondaryChildTech
                );



        }
    }
}
