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
        public ActionResult UpdateStationAttempts(long stationId, long ringId, int attempts, bool broke)
        {
            var result = _orch.UpdateStationAttempts(stationId, attempts, !broke);
            
            if (result.WasSuccessful)
            {
                //AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToRing(ringId);
        }

        [HttpPost]
        public ActionResult UpdateWeaponOrFormScoring(long entryId, int judgeScore, bool isWeapon, long ringId)
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
            return RedirectToRing(ringId);
        }

        [HttpPost]
        public ActionResult NewWeaponOrFormScoring(long ringId, long partId, bool isWeapon)
        {
            OperationResult result;

            if(isWeapon){
                result = _orch.NewWeaponEntry(ringId, partId);
            }else{
                result = _orch.NewFormEntry(ringId, partId);
            }
            
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToRing(ringId);
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

        private BreakingViewModel mkBreakingViewModel(long ringId)
        {
            var vm = new BreakingViewModel();
            vm.RingId = ringId;
            vm.Participants = _orch.GetParticipantsByRingId(ringId);
            vm.SelectedParticipantId = -1;
            var svm = mkStationViewModel();
            for (int i = StoredValues.MaxBreakingStationCount; i > 0;--i )
            {
                //tried to limit this with a deepcopy, it didn't work . . .
                vm.Stations.Add( mkStationViewModel() );
            }
            return vm;
        }


        public ActionResult CreateBreakingEntry(long ringId)
        {
            BreakingViewModel vm = mkBreakingViewModel(ringId);

            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateBreakingEntry(BreakingViewModel vm)
        {
            var model = new BreakingResult();
            model.Ring = _orch.GetRingById(vm.RingId);
            model.Participant = _orch.GetParticipantById(vm.SelectedParticipantId);
            foreach(var stationVM in vm.Stations){
                var result = _orch.CreateTechniqueValue(stationVM.BaseTechniques);
                if(result.WasSuccessful && result.HasTechniqueValue){
                    model.Stations.Add(new Station()
                    {
                        Attempts = stationVM.Attempts,
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

            return RedirectToRing(vm.RingId);
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

            var result = _orch.EnterBreakingJudgeScore(score, vm.EntryId);

            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
                return RedirectToHome();
            }

            return RedirectToRing(result.RingId );
        }

        [HttpPost]
        public ActionResult DeleteBreakingEntry(long ringId, long entryId)
        {
       

            var result = _orch.DeleteBreakingEntry(entryId);

            if (result.WasSuccessful)
            {
                //AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }

            return RedirectToRing(ringId);
        }

    }
}
