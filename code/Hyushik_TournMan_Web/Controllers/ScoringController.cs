using Hyushik_TournMan_Web.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;

namespace Hyushik_TournMan_Web.Controllers
{
    public class ScoringController : BaseController
    {

        private IScoringOrchestrator _orch = new ScoringOrchestrator();

        //
        // GET: /Scoring/

        private StationViewModel mkStationViewModel()
        {
            var vm = new StationViewModel();
            vm.BaseTechniques = _orch.GetTopLevelTechniques().ToList<Technique>();
            vm.BoardsViewModel = new BoardsViewModel();
            return vm;
        }

        private BreakingViewModel mkBreakingViewModel(long tournId)
        {
            var vm = new BreakingViewModel();
            vm.TournamentId = tournId;
            vm.Participants = _orch.GetParticipantsByTournId(tournId);
            vm.SelectedParticipantId = -1;
            var svm = mkStationViewModel();
            //TODO remove magic number
            for (int i = 5; i > 0;--i )
            {
                //tried to limit this with a deepcopy, it didn't work . . .
                vm.Stations.Add( mkStationViewModel() );
            }
            return vm;
        }


        public ActionResult CreateBreakingEntry(long tournId)
        {
            BreakingViewModel vm = mkBreakingViewModel(tournId);

            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateBreakingEntry(BreakingViewModel vm)
        {
            var model = new BreakingResult();
            model.Tournament = _orch.GetTournamentById(vm.TournamentId);
            model.Participant = _orch.GetParticipantById(vm.SelectedParticipantId);
            foreach(var stationVM in vm.Stations){
                var result = _orch.CreateTechniqueValue(stationVM.BaseTechniques);
                if(result.WasSuccessful && result.HasTechniqueValue){
                    model.Stations.Add(result.TechniqueValue);
                }
            }
            var saveResult = _orch.SaveBreakingResult(model);
            if (saveResult.WasSuccessful)
            {
                AddSucessNotification(saveResult.Message);
            }
            else if (!saveResult.WasSuccessful)
            {
                AddErrorNotification(saveResult.Message);
            }

            return RedirectToAction("Index", "ActiveTournament", new { tournId = vm.TournamentId });
        }

    }
}
