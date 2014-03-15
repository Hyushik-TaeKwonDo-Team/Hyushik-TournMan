using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class BreakingViewModel
    {

        public ParticipantSelection ParticipantSelection { get; set; }

        //default 5
        public List<StationViewModel> Stations { get; set; }
        public long RingId { get; set; }

        public BreakingViewModel()
        {
            Stations = new List<StationViewModel>();
        }
    }

    public class StationViewModel {
        //way more data passed, but ease of editing
        public List<Technique> BaseTechniques { get; set; }
        public int Attempts { get; set; }
        public BoardsViewModel BoardsViewModel { get; set; }

        public StationViewModel()
        {
            Attempts = 1;
        }
        
    }

    public class BoardsViewModel
    {
        public double Width { get; set; }
        public double Depth { get; set; }
        public int Amount { get; set; }
        public bool SpeedBreak { get; set; }
        public bool Spacers { get; set; }
        public List<double> PossibleBoardWidths { get; set; }
        public List<double> PossibleBoardDepths { get; set; }

        public BoardsViewModel()
        {
            Amount = 1;
        }
    }

    public class JudgeBreakingScoringViewModel
    {
        public long EntryId { get; set; }
        [Range(minimum: -5, maximum: 5, ErrorMessage = "Subjective score must be between -5 and 5.")]
        public int SubjectiveScore { get; set; }
    }
}