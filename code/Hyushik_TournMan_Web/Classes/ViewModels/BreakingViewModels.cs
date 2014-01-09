using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class BreakingViewModel
    {
        //default 5
        public List<StationViewModel> Stations { get; set; }

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

    }

    public class BoardsViewModel
    {
        public double Height { get; set; }
        public double Depth { get; set; }
        public int Amount { get; set; }
        public bool SpeedBreak { get; set; }
        public bool Spacers { get; set; }
    }
}