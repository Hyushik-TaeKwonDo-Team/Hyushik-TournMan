using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class RingViewModel
    {
        public Ring Ring { get; set; }
        
        public BreakingScoreListingViewModel BreakingScoreListingViewModel { get; set; }
        public WeaponsOrFormsListingViewModel WeaponsListingViewModel { get; set; }
        public WeaponsOrFormsListingViewModel FormsListingViewModel { get; set; }
        public SparringViewModel SparringViewModel { get; set; }
    }
}