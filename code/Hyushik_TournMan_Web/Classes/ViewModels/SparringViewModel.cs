using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class SparringViewModel
    {
        public List<SparringResult> SparringResults { get; set; }

        public ParticipantSelection Participant1Selection { get; set; }
        public ParticipantSelection Participant2Selection { get; set; }
        public bool Participant1IsVictor {get; set;}

        public Ring Ring { get; set; }
        public int RoundNumber { get; set; }

        public SparringViewModel()
        {
            SparringResults = new List<SparringResult>();
            Participant1IsVictor = false;
            RoundNumber = 1;
        }
    }
}