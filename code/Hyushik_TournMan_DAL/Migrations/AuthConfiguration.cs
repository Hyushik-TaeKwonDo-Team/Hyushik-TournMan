namespace Hyushik_TournMan_DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebMatrix.WebData;
    using System.Web.Security;
    using Hyushik_TournMan_Common.Constants;

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

            var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;
            
            if (!roles.RoleExists(Constants.Roles.ADMINISTRATOR_ROLE))
                roles.CreateRole(Constants.Roles.ADMINISTRATOR_ROLE);

            if (!roles.RoleExists(Constants.Roles.JUDGE_ROLE))
                roles.CreateRole(Constants.Roles.JUDGE_ROLE);

            if (!WebSecurity.UserExists("admin"))
                WebSecurity.CreateUserAndAccount(
                    "admin",
                    "password");

            if (!WebSecurity.UserExists("judge"))
                WebSecurity.CreateUserAndAccount(
                    "judge",
                    "password");

            if (!roles.GetRolesForUser("admin").Contains(Constants.Roles.ADMINISTRATOR_ROLE))
                roles.AddUsersToRoles(new[] { "admin" }, new[] { Constants.Roles.ADMINISTRATOR_ROLE });

            if (!roles.GetRolesForUser("judge").Contains(Constants.Roles.JUDGE_ROLE))
                roles.AddUsersToRoles(new[] { "judge" }, new[] { Constants.Roles.JUDGE_ROLE });
             

        }
    }
}
