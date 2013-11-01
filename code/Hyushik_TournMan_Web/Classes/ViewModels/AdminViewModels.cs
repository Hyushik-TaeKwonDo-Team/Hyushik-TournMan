using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class AdminViewModel
    {
        public TournamentsViewModel TournamentViewModel { get; set; }
        public ImportCsvViewModel ImportCsvViewModel { get; set; }
    }


    public class TournamentsViewModel
    {
        public TournamentsViewModel()
        {
            NewTournamentName = string.Empty;
        }

        [DisplayName("Add New Tournament")]
        public string NewTournamentName { get; set; }

        [DisplayName("Tournaments")]
        public IList<Tournament> Tournaments { get; set; } 
    }


    public class ImportCsvViewModel
    {
        [DisplayName("Participants CSV File")]
        public HttpPostedFileBase CsvFile { get; set; }

        public IList<Tournament> Tournaments { get; set; }

        [DisplayName("Tournament")]
        public long SelectedTournamentId { get; set; }

        public IEnumerable<SelectListItem> TournamentItems
        {
            get
            {
                return Tournaments.Select(t => new SelectListItem
                                               {
                                                   Value = t.Id.ToString(),
                                                   Text = t.Name
                                               });
            }
        }

    }

    public class ParticipantsViewModel
    {
        public Tournament Tournament { get; set; }
    }
}