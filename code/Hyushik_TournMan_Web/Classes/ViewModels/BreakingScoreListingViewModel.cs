﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class BreakingScoreListingViewModel
    {
        public long TournamentId { get; set; }

        public List<BreakingScoreListing> BreakingScoreListings { get; set; }

        public BreakingScoreListingViewModel()
        {
            BreakingScoreListings = new List<BreakingScoreListing>();
        }

        public void AddListing(string name, long id, double currentScore){
            BreakingScoreListings.Add(
                new BreakingScoreListing{
                    ParticipantName=name,
                    BreakingEntryId=id,
                    CurrentScore = currentScore
                }
            );
        }
    }

    public class BreakingScoreListing{
        public string ParticipantName {get; set;}
        public double CurrentScore { get; set; }
        public long BreakingEntryId {get; set;}
    }
}