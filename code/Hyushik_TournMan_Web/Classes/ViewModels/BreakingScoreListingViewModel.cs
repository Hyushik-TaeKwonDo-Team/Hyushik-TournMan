﻿using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class BreakingScoreListingViewModel
    {
        public Ring Ring { get; set; }

        public List<BreakingScoreListing> BreakingScoreListings { get; set; }

        public BreakingScoreListingViewModel()
        {
            BreakingScoreListings = new List<BreakingScoreListing>();
        }

        public void AddListing(string name, long id, double currentScore, List<Station> stations, Dictionary<long, string> judgeIdToName, Dictionary<long, int> judgeIdToScore)
        {
            BreakingScoreListings.Add(
                new BreakingScoreListing{
                    ParticipantName=name,
                    BreakingEntryId=id,
                    CurrentScore = currentScore,
                    Stations = stations,
                    JudgeIdToName = judgeIdToName,
                    JudgeIdToScore = judgeIdToScore
                }
            );
        }
    }

    public class BreakingScoreListing{
        public string ParticipantName {get; set;}
        public double CurrentScore { get; set; }
        public long BreakingEntryId {get; set;}
        public List<Station> Stations { get; set; }
        public Dictionary<long, string> JudgeIdToName { get; set; }
        public Dictionary<long, int> JudgeIdToScore { get; set; }

        public double JudgeScoreTieBreaker()
        {
            if (JudgeIdToScore.Values.Count<1)
            {
                return 0;
            }
            return JudgeIdToScore.Values.Average();
        }

        public double StationCountTiebreaker()
        {
            return Stations.Count;
        }
    }
}