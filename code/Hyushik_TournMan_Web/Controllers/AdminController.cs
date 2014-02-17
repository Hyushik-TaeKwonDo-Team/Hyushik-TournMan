using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_Common.Results;
using Hyushik_TournMan_Web.Classes.Constants;
using Hyushik_TournMan_Common.Constants;
using Hyushik_TournMan_Common.Properties;
using Hyushik_TournMan_Web.Classes.ViewModels;
using Hyushik_TournMan_Web.Filters;

namespace Hyushik_TournMan_Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [InitializeSimpleMembership]
    public class AdminController : BaseController
    {
        private IAdminOrchestrator orch = new AdminOrchestrator();

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View(_buildAdminVM());
        }

        private BreakingStoredValuesViewModel _breakingStoredValuesViewModel()
        {
            var vm = new BreakingStoredValuesViewModel()
            {
                //convert from double
                StationFalloffProportion = (int)(orch.GetStationFalloffProportion()*100),
                MaxBreakingStationCount = orch.GetMaxBreakingStationCount(),
                PossibleBoardWidths = orch.GetPossibleBoardWidthsAsString(),
                PossibleBoardDepths = orch.GetPossibleBoardDepthsAsString()
            };
            return vm;
        }

        private ImportCsvViewModel _buildImportCsvVM()
        {
            var vm = new ImportCsvViewModel()
            {
                Tournaments=orch.GetAllTournaments()
            };
            return vm;
        }

        private TechniquesViewModel _buildTechniquesVM()
        {
            var vm = new TechniquesViewModel()
            {
                Techniques = orch.GetTopLevelTechniques().ToList()
            };
            return vm;
        }

        private TournamentsViewModel _buildTournamentsVM()
        {
            var vm = new TournamentsViewModel()
            {
                Tournaments = orch.GetAllTournaments()
            };
            return vm;
        }

        private UserRolesViewModel _buildUserRolesVM()
        {
            var vm = new UserRolesViewModel()
            {
                Users = orch.GetAllUsers(),
                UserNameToRoles = orch.GetMappingOfUserNameToRoles()
            };

            return vm;
        }

        private AdminViewModel _buildAdminVM()
        {
            var vm = new AdminViewModel()
            {
                ImportCsvViewModel = _buildImportCsvVM(),
                TournamentViewModel = _buildTournamentsVM(),
                UserRolesViewModel = _buildUserRolesVM(),
                TechniquesViewModel = _buildTechniquesVM(),
                BreakingStoredValuesViewModel = _breakingStoredValuesViewModel()
            };
            return vm;
        }

        [HttpPost]
        public ActionResult ImportCsv(ImportCsvViewModel vm)
        {
            // Verify that the user selected a file
            OperationResult result;
            var file = vm.CsvFile;
            if (file != null && file.ContentLength > 0)
            {
                result = orch.ImportParticipantCsvFile(file.InputStream, vm.SelectedTournamentId);
            }
            else
            {
                result =  new OperationResult()
                {
                    WasSuccessful = false,
                    Message = Resources.FileUnreadableMessage
                };
            }


            var resultMessages = new Dictionary<string, string>();
            string notificationClass = String.Empty;
            if (result.WasSuccessful){
                AddSucessNotification(result.Message);
            }else if(!result.WasSuccessful){
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Tournament(long id)
        {
            var vm = new TournamentViewModel()
            {
                Tournament = orch.GetTournamentById(id),
                TotalBoardCounts = orch.GetTotalBoardSizeCountsByTournamentId(id)

            };

            return View(vm);
        }

        [HttpGet]
        public ActionResult Participant(long id)
        {
            var vm = new ParticipantViewModel()
            {
                Participant = orch.GetParticipantById(id)
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddTournament(TournamentsViewModel vm)
        {
            var result = orch.CreateNewTournament(vm.NewTournamentName);

            var resultMessages = new Dictionary<string, string>();
            string notificationClass = String.Empty;
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddRole(string userName, string roleName)
        {
            orch.AddRole(userName, roleName);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RevokeRole(string userName, string roleName)
        {
            orch.RemoveRole(userName, roleName);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetTournamentActiveStatus(long tournId, bool tournActive)
        {
            var result = orch.SetTournamentActiveStatus(tournId, tournActive);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTechnique(long techId, string techName, int techWeight, bool techToggleable)
        {
            var result = orch.UpdateTechnique(techId, techName, techWeight, techToggleable);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddTechnique(long parentId, string techName, int techWeight, bool techToggleable)
        {
            var result = orch.AddTechnique(parentId, techName, techWeight, techToggleable);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteTechnique(long techId)
        {
            var result = orch.DeleteTechnique(techId);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetBreakingStoredValues(BreakingStoredValuesViewModel vm)
        {
            var results = new List<OperationResult>();
            //d is for double, that's good enough for me
            orch.SetStationFalloffProportion(vm.StationFalloffProportion / 100d);
            orch.SetStationMaxBreakingStationCount(vm.MaxBreakingStationCount);
            results.Add(orch.SetPossibleBoardWidths(vm.PossibleBoardWidths));
            results.Add(orch.SetPossibleBoardDepths(vm.PossibleBoardDepths));

            foreach (var result in results){
                if(String.IsNullOrEmpty(result.Message)){
                    continue; 
                }
                if (result.WasSuccessful)
                {
                    AddSucessNotification(result.Message);
                }
                else if (!result.WasSuccessful)
                {
                    AddErrorNotification(result.Message);
                }
            }

            return RedirectToAction("Index");   
        }
    }
}
