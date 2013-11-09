using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using Hyushik_TournMan_Common.Models;

namespace Hyushik_TournMan_DAL.Contexts
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("HyushikUsers")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
