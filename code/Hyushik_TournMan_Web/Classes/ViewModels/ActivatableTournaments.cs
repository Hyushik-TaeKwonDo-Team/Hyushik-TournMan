using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class ActivatableTournamentsViewModel
    {
        public IList<Tournament> Tournaments { get; set; }
    }
}