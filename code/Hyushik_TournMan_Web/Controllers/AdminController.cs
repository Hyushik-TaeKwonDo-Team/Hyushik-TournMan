﻿using System;
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
    [Authorize(Roles = Constants.Roles.ADMINISTRATOR_ROLE)]
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

        public ActionResult ParticipantCard(long partId)
        {
            return View(_buildParticipantCardViewModel(partId));
        }

        public ActionResult ParticipantCards(long tournId)
        {
            return View(_buildParticipantCardViewModelList(tournId));
        }

        private ParticipantCardViewModel _buildParticipantCardViewModel(long partId)
        {
            return new ParticipantCardViewModel()
            {
                Participant = orch.GetParticipantById(partId),
                QRCodeBase64Img = orch.GetQRCodeBase64StringFromLong(partId)
            };
        }

        private List<ParticipantCardViewModel> _buildParticipantCardViewModelList(long tournId)
        {
            var tourn = orch.GetTournamentById(tournId);
            var vmList = new List<ParticipantCardViewModel>();

            foreach (var partId in tourn.Participants.Select(p=>p.ParticipantId)){
                vmList.Add(_buildParticipantCardViewModel(partId));
            }

            return vmList;

        }



        private RingParticipantSelectionViewModel _buildRingParticipantSelectionViewModel(long tournId)
        {
            var tourn = orch.GetTournamentById(tournId);
            var vm = new RingParticipantSelectionViewModel(tourn, tourn.Rings, tourn.Participants);
            return vm;
        }

        private BreakingStoredValuesViewModel _breakingStoredValuesViewModel()
        {
            var vm = new BreakingStoredValuesViewModel()
            {
                //convert from double
                PossibleBoardWidths = orch.GetPossibleBoardWidthsAsString(),
                PossibleBoardDepths = orch.GetPossibleBoardDepthsAsString(),
                BreakingBoardExponent = orch.GetBreakingBoardExponent(),
                BreakingMaxStationCount = orch.GetBreakingMaxStationCount(),
                BreakingMaximumBoards = orch.GetBreakingMaximumBoards(),
                BreakingMaximumAttempts = orch.GetBreakingMaximumAttempts(),
                BreakingAttemptDecayRate = orch.GetBreakingAttemptDecayRate(),
                BreakingSpacerPenalty = orch.GetBreakingSpacerPenalty(),
                BreakingPowerHoldPenalty = orch.GetBreakingPowerHoldPenalty(),
                BreakingJudgeWeight = orch.GetBreakingJudgeWeight(),
                BreakingMaxScore = orch.GetBreakingMaxScore()

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
        public ActionResult UpdateTechnique(long techId, string techName, double techWeight)
        {
            var result = orch.UpdateTechnique(techId, techName, Convert.ToInt32(techWeight));
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
        public ActionResult AddTechnique(long parentId, string techName, double techWeight)
        {
            var result = orch.AddTechnique(parentId, techName, Convert.ToInt32(techWeight));
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
            

                orch.SetBreakingBoardExponent(vm.BreakingBoardExponent);
                orch.SetBreakingMaxStationCount(Convert.ToInt32(vm.BreakingMaxStationCount));
                orch.SetBreakingMaximumBoards(Convert.ToInt32(vm.BreakingMaximumBoards));
                orch.SetBreakingMaximumAttempts(Convert.ToInt32(vm.BreakingMaximumAttempts));
                orch.SetBreakingAttemptDecayRate(vm.BreakingAttemptDecayRate);
                orch.SetBreakingSpacerPenalty(vm.BreakingSpacerPenalty);
                orch.SetBreakingPowerHoldPenalty(vm.BreakingPowerHoldPenalty);
                orch.SetBreakingJudgeWeight(vm.BreakingJudgeWeight);
                orch.SetBreakingMaxScore(Convert.ToInt32(vm.BreakingMaxScore));

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

        public ActionResult AddParticipantsToRings(long tournId)
        {

            return View(_buildRingParticipantSelectionViewModel(tournId));
        }


        [HttpPost]
        public ActionResult AddParticipantsToRings(RingParticipantSelectionViewModel vm)
        {
            orch.AddParticipantsToRings(vm.Rings.Select(r => r.Id).ToList(), vm.Participants.Select(p => p.ParticipantId).ToList(), vm.PartRingJoin);
            return RedirectToTournamentAdmin(vm.Tournament.Id);
        }

        [HttpPost]
        public ActionResult AddIndividualParticipant(long tournId, string firstLastName, string optionsRadios, double age, string rank, string weapons, string breaking, string forms, string point, string olympic)
        {
            AddSucessNotification("Added " + firstLastName + " to the tournament.");
            String[] participantInfo = {firstLastName, optionsRadios, Convert.ToInt32(age).ToString(), rank};
            bool[] events = {false, false, false, false, false};
            if (weapons != null)
                events[0] = true;
            if (breaking != null)
                events[1] = true;
            if (forms != null)
                events[2] = true;
            if (point != null)
                events[3] = true;
            if (olympic != null)
                events[4] = true;
            orch.AddIndividualParticipant(tournId, participantInfo, events);
            return RedirectToTournamentAdmin(tournId);
        }

        [HttpPost]
        public ActionResult DeleteRing(long ringId, long tournId)
        {
            var result = orch.DeleteRing(ringId);
            if (result.WasSuccessful)
            {
                //AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToTournamentAdmin(tournId);
        }

        [HttpPost]
        public ActionResult CreateRing(string ringName, long tournId)
        {
            var result = orch.CreateRing(ringName, tournId);
            if (result.WasSuccessful)
            {
                //AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToTournamentAdmin(tournId);
        }

        protected ActionResult RedirectToTournamentAdmin(long tournId){
            return RedirectToAction("Tournament", "Admin", new { id = tournId });
        }

    }
}
