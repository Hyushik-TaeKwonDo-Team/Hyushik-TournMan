using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class WeaponsOrFormsListingViewModel
    {
        public long RingId { get; set; }
        public List<WeaponsOrFormsListing> WeaponsOrFormsListings { get; set; }
        public bool IsWeapons { get; set; }
        public ParticipantSelection ParticipantSelection { get; set; }

        public WeaponsOrFormsListingViewModel()
        {
            WeaponsOrFormsListings = new List<WeaponsOrFormsListing>();
        }

        public void AddListing(string name, double score, long resultId)
        {
            WeaponsOrFormsListings.Add(new WeaponsOrFormsListing()
            {
                ParticipantName=name,
                CurrentScore=score,
                WeaponsOrFormsResultId=resultId,
            });
        }

    }

    public class WeaponsOrFormsListing{
        public string ParticipantName {get; set;}
        public double CurrentScore { get; set; }
        public long WeaponsOrFormsResultId {get; set;}
    }
}