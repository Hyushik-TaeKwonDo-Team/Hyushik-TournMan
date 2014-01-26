﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class BreakingScoreListingViewModel
    {
        public long TournamentId;

        public List<BreakingScoreListing> BreakingScoreListings { get; set; }

        public BreakingScoreListingViewModel()
        {
            BreakingScoreListings = new List<BreakingScoreListing>();
        }

        public void AddListing(string name, long id){
            BreakingScoreListings.Add(
                new BreakingScoreListing{
                    ParticipantName=name,
                    BreakingEntryId=id
                }
            );
        }
    }

    public class BreakingScoreListing{
        public string ParticipantName {get; set;}
        public long BreakingEntryId {get; set;}
    }
}