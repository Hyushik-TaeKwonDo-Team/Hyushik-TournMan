namespace Hyushik_TournMan_DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebMatrix.WebData;
    using System.Web.Security;

    internal sealed class AuthConfiguration : DbMigrationsConfiguration<Hyushik_TournMan_DAL.Contexts.UsersContext>
    {
        public AuthConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Hyushik_TournMan_DAL.Contexts.UsersContext context)
        {

            WebSecurity.InitializeDatabaseConnection(
                "HyushikUsers",
                "UserProfile",
                "UserId",
                "UserName", autoCreateTables: true);

            var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;;

            if (!roles.RoleExists("Administrator"))
                roles.CreateRole("Administrator");

            if (!roles.RoleExists("Judge"))
                roles.CreateRole("Judge");

            if (!WebSecurity.UserExists("admin"))
                WebSecurity.CreateUserAndAccount(
                    "admin",
                    "password");

            if (!WebSecurity.UserExists("judge"))
                WebSecurity.CreateUserAndAccount(
                    "judge",
                    "password");

            if (!roles.GetRolesForUser("admin").Contains("Administrator"))
                roles.AddUsersToRoles(new[] { "admin" }, new[] { "Administrator" });

            if (!roles.GetRolesForUser("judge").Contains("Judge"))
                roles.AddUsersToRoles(new[] { "judge" }, new[] { "Judge" });

        }
    }
}
