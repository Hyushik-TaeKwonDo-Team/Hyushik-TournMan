using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using Hyushik_TournMan_Common.Models;

namespace Hyushik_TournMan_DAL.Contexts
{
    public class TournManContext : DbContext
    {
        
        public TournManContext()
            : base("HyushikDB")
        {
        }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<BoardSizeCount> BoardSizeCounts { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

    }
}
