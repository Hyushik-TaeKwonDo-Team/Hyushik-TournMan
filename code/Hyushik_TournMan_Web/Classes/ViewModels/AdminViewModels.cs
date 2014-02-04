using Hyushik_TournMan_Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hyushik_TournMan_Web.Classes.ViewModels
{
    public class AdminViewModel
    {
        public TournamentsViewModel TournamentViewModel { get; set; }
        public ImportCsvViewModel ImportCsvViewModel { get; set; }
        public UserRolesViewModel UserRolesViewModel { get; set; }
        public TechniquesViewModel TechniquesViewModel { get; set; }
        public BreakingStoredValuesViewModel BreakingStoredValuesViewModel { get; set;  }
    }

    public class TechniquesViewModel
    {
        public List<Technique> Techniques { get; set; } 
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

    public class TournamentViewModel
    {
        public Tournament Tournament { get; set; }

        [DisplayName("Boards Needed for Tournament")]
        public IList<BoardSizeCount> TotalBoardCounts { get; set; }
    }

    public class BreakingStoredValuesViewModel
    {
        [Range(minimum: 1, maximum: 100, ErrorMessage = "Percentage must be between 1 and 100.")]
        public int StationFalloffProportion { get; set; }
        [Range(minimum: 1, maximum: 20, ErrorMessage = "Count must be between 1 and 20.")]
        public int MaxBreakingStationCount { get; set; }
        public string PossibleBoardWidths { get; set; }
        public string PossibleBoardDepths { get; set; }
    }
    public class ParticipantViewModel
    {
        public Participant Participant { get; set; }
    }
       
}