using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class RingParticipantSelectionViewModel
    {
        public Tournament Tournament { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Ring> Rings { get; set; }
        //x = rings, y = parts 
        public List<List<bool>> RingsVsParticipants { get; set; }

        public RingParticipantSelectionViewModel()
        {
            Rings = new List<Ring>();
            Participants = new List<Participant>();
            RingsVsParticipants = new List<List<bool>>();
        }

        public RingParticipantSelectionViewModel(Tournament tourn, List<Ring> rings, List<Participant> participants)
        {
            Tournament = tourn;
            RingsVsParticipants = new List<List<bool>>();
            Rings = rings;
            Participants = participants;
            //build the matrix
            foreach(var part in participants){
                var partList = new List<bool>();
                partList.AddRange(Enumerable.Repeat(false, rings.Count));
                for (var i = 0; i < rings.Count; ++i )
                {
                    if (rings.ElementAt(i).SelectedParticipants.Contains(part))
                    {
                        partList.Insert(i, true);
                    }
                }
                RingsVsParticipants.Add(partList);
            }


        }


    }
}