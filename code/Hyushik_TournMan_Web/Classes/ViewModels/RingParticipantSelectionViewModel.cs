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
        public List<List<bool>> PartRingJoin { get; set; }

        public RingParticipantSelectionViewModel()
        {
            Rings = new List<Ring>();
            Participants = new List<Participant>();
            PartRingJoin = new List<List<bool>>();
        }

        public RingParticipantSelectionViewModel(Tournament tourn, List<Ring> rings, List<Participant> participants)
        {
            Tournament = tourn;
            PartRingJoin = new List<List<bool>>();
            Rings = rings.OrderBy(r => r.Name).ToList();
            Participants = participants.OrderBy(r => r.Name).ToList();
            //build the matrix
            foreach(var part in Participants){
                var partList = new List<bool>();
                partList.AddRange(Enumerable.Repeat(false, Rings.Count));
                for (var i = 0; i < Rings.Count; ++i )
                {
                    if (rings[i].SelectedParticipants.Exists(p=>p.ParticipantId==part.ParticipantId))
                    {
                        partList[i]= true;
                    }
                }
                PartRingJoin.Add(partList);
            }


        }


    }
}