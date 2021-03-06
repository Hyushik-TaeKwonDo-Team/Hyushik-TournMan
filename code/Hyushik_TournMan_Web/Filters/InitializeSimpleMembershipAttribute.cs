﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_DAL.Contexts;
using Hyushik_TournMan_Common.Constants;
using System.Web.Security;

using System.Linq;

namespace Hyushik_TournMan_Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("HyushikUsers", "UserProfile", "UserId", "UserName", autoCreateTables: true);


                    //Creating default admin
                    var roles = (WebMatrix.WebData.SimpleRoleProvider)Roles.Provider;

                    if (!roles.RoleExists(Constants.Roles.ADMINISTRATOR_ROLE))
                        roles.CreateRole(Constants.Roles.ADMINISTRATOR_ROLE);

                    if (!roles.RoleExists(Constants.Roles.JUDGE_ROLE))
                        roles.CreateRole(Constants.Roles.JUDGE_ROLE);

                    if (!WebSecurity.UserExists(Constants.DefaultAdmin.USERNAME) && 0 == roles.FindUsersInRole(Constants.Roles.ADMINISTRATOR_ROLE, String.Empty).Length)
                    {
                        WebSecurity.CreateUserAndAccount(
                            Constants.DefaultAdmin.USERNAME,
                            Constants.DefaultAdmin.PASSWORD);
                        if (!roles.GetRolesForUser(Constants.DefaultAdmin.USERNAME).Contains(Constants.Roles.ADMINISTRATOR_ROLE))
                            roles.AddUsersToRoles(new[] { Constants.DefaultAdmin.USERNAME }, new[] { Constants.Roles.ADMINISTRATOR_ROLE });
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
