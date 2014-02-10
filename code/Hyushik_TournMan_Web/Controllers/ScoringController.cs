using Hyushik_TournMan_Web.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Web.Filters;
using Hyushik_TournMan_DAL.StoredValues;
using Hyushik_TournMan_Common.Results;

namespace Hyushik_TournMan_Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Judge")]
    public class ScoringController : BaseController
    {

        private IScoringOrchestrator _orch = new ScoringOrchestrator();

        //
        // GET: /Scoring/

        [HttpPost]
        public ActionResult UpdateWeaponOrFormScoring(long entryId, int judgeScore, bool isWeapon, long tournId)
        {
            OperationResult result;

            if (isWeapon)
            {
                result = _orch.ScoreWeaponEntry(entryId, judgeScore, User.Identity.Name);
            }
            else
            {
                result = _orch.ScoreFormEntry(entryId, judgeScore, User.Identity.Name);
            }

            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index", "ActiveTournament", new { tournId = tournId });
        }

        [HttpPost]
        public ActionResult NewWeaponOrFormScoring(long tournId, long partId, bool isWeapon)
        {
            OperationResult result;

            if(isWeapon){
                result = _orch.NewWeaponEntry(tournId, partId);
            }else{
                result = _orch.NewFormEntry(tournId, partId);
            }
            
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index", "ActiveTournament", new { tournId = tournId });
        }

        private StationViewModel mkStationViewModel()
        {
            var vm = new StationViewModel();
            vm.BaseTechniques = _orch.GetTopLevelTechniques().ToList<Technique>();
            vm.BoardsViewModel = new BoardsViewModel();
            vm.BoardsViewModel.PossibleBoardWidths = _orch.GetPossibleBoardWidths();
            vm.BoardsViewModel.PossibleBoardDepths = _orch.GetPossibleBoardDepths();
            return vm;
        }

        protected JudgeBreakingScoringViewModel mkJudgeBreakingScoringViewModel(long entryId){
            return new JudgeBreakingScoringViewModel()
            {
                EntryId = entryId
            };
        }

        private BreakingViewModel mkBreakingViewModel(long tournId)
        {
            var vm = new BreakingViewModel();
            vm.TournamentId = tournId;
            vm.Participants = _orch.GetParticipantsByTournId(tournId);
            vm.SelectedParticipantId = -1;
            var svm = mkStationViewModel();
            for (int i = StoredValues.MaxBreakingStationCount; i > 0;--i )
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
                    model.Stations.Add(new Station()
                    {
                        attempts = stationVM.Attempts,
                        BoardCount = stationVM.BoardsViewModel.Amount,
                        BoardWidth = stationVM.BoardsViewModel.Width,
                        BoardDepth = stationVM.BoardsViewModel.Depth,
                        BoardSpacers = stationVM.BoardsViewModel.Spacers,
                        Technique = result.TechniqueValue
                    });
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

        public ActionResult JudgeBreakingEntry(long entryId)
        {
            return View(mkJudgeBreakingScoringViewModel(entryId));
        }

        [HttpPost]
        public ActionResult JudgeBreakingEntry(JudgeBreakingScoringViewModel vm)
        {
            var score = new BreakingJudgeScore();
            score.SubjectiveScore=vm.SubjectiveScore;
            score.Judge_UserId = _orch.GetUsers().First(u=>u.UserName==User.Identity.Name).UserId;

            var result = _orch.EnterJudgeScore(score, vm.EntryId);

            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "ActiveTournament", new { tournId = result.TournamentId });
        }

    }
}
