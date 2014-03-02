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
            : base("HyushikTournament")
        {
        }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<BoardSizeCount> BoardSizeCounts { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Ring> Rings { get; set; }

        public DbSet<Technique> Techniques { get; set; }
        public DbSet<BreakingResult> BreakingResults { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<BreakingJudgeScore> BreakingJudgeScores { get; set; }
        public DbSet<TechniqueValue> TechniqueValues { get; set; }

        public DbSet<WeaponResult> WeaponResults { get; set; }
        public DbSet<FormResult> FormResults { get; set; }
        public DbSet<WeaponAndFormJudgeScore> WeaponAndFormJudgeScores { get; set; }
        
        

    }
}
