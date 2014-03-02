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

            var part1 = new Participant() { Name = "Participant 1" };
            var part2 = new Participant() { Name = "Participant 2" };
            var part3 = new Participant() { Name = "Participant 3" };
            context.Participants.AddOrUpdate(
                p => p.Name,
                part1, part2, part3
                );

            var tourn1 = new Tournament() { Name = "Tournament 1", Active = true };

            var ring1 = new Ring() { Name = "Ring 1", Tournament=tourn1};
            var ring2 = new Ring() { Name = "Ring 2", Tournament = tourn1 };

            
            tourn1.Participants.Add(part1);
            tourn1.Participants.Add(part2);
            tourn1.Participants.Add(part3);

            tourn1.Rings.Add(ring1);
            tourn1.Rings.Add(ring2);
            
            context.Tournaments.AddOrUpdate(
                t=> t.Name,
                tourn1,
                new Tournament(){Name="Tournament 2"}
                );

            context.Rings.AddOrUpdate(
                r => r.Name,
                ring1,
                ring2
                );

            var toggleTech = new Technique()
            {
                Name = "Toggle Tech"
            };

            var parentTech = new Technique()
            {
                Name = "Parent Tech"
            };

            var childTech = new Technique()
            {
                Name = "Child Tech"
            };

            var secondaryChildTech = new Technique()
            {
                Name = "Secondary Child Tech"
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
