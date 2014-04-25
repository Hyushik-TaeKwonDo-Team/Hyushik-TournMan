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
using Hyushik_TournMan_Common.Properties;

namespace Hyushik_TournMan_Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Judge")]
    public class ScoringController : BaseController
    {

        private IScoringOrchestrator _orch = new ScoringOrchestrator();

        [HttpPost]
        public ActionResult DeleteFormResult(long ringId, long worfId)
        {
            var result = _orch.DeleteFormsResult(worfId);

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
        public ActionResult DeleteWeaponResult(long ringId, long worfId)
        {
            var result = _orch.DeleteWeaponsResult(worfId);

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
        public ActionResult DeleteSparringEntry(long ringId, long sparId)
        {
            var result = _orch.DeleteSparringResult(sparId);

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
        public ActionResult SaveSparringResult(SparringViewModel vm)
        {
            try
            {
                var sparResult = new SparringResult()
                {
                    Participant1 = _orch.GetParticipantById(_orch.GetParticipantIdBySelection(vm.Participant1Selection)),
                    Participant2 = _orch.GetParticipantById(_orch.GetParticipantIdBySelection(vm.Participant2Selection)),
                    Ring = _orch.GetRingById(vm.Ring.Id),
                    RoundNumber = vm.RoundNumber,
                    Partipant1IsVictor = vm.Participant1IsVictor
                };
                var result = _orch.SaveSparringResult(sparResult);

                if (result.WasSuccessful)
                {
                    //AddSucessNotification(result.Message);
                }
                else if (!result.WasSuccessful)
                {
                    AddErrorNotification(result.Message);
                }
            }
            catch (InvalidOperationException ex)
            {
                var exMess = ex.Message;
                AddErrorNotification(Resources.ParticipantNotSelectedMessage);
            }
            catch (Exception ex)
            {
                AddErrorNotification(ex.Message);
            }
            
            return RedirectToRing(vm.Ring.Id);
        }


        protected ParticipantSelection MkParticipantSelection(long ringId)
        {

            var result = _orch.GetParticipantSelectionByRingId(ringId);
            if (result.WasSuccessful)
            {
                return result.ParticipantSelection;
            }
            else
            {
                //AddErrorNotification(result.Message);
                return null;
            }


        }

        //
        // GET: /Scoring/

        [HttpPost]
        public ActionResult UpdateStationAttempts( List<long> stationIds, long ringId, List<int> attempts, List<bool> broke)
        {

            //this is what is known as a "cheap hack" as the code freeze is in five days
            var results = new List<OperationResult>();
            for (int i = 0; i<stationIds.Count(); ++i){
                var result = _orch.UpdateStationAttempts(stationIds[i], attempts[i], !(broke[i]));
                results.Add(result);
            }

            if (results.Exists(r=>!r.WasSuccessful))
            {
                AddErrorNotification(results.First(r => !r.WasSuccessful).Message);
            }
            return RedirectToRing(ringId);
        }

        [HttpPost]
        public ActionResult UpdateWeaponOrFormScoring(long entryId, double judgeScore, bool isWeapon, long ringId)
        {
            OperationResult result;

            if (isWeapon)
            {
                result = _orch.ScoreWeaponEntry(entryId, Convert.ToInt32(judgeScore), User.Identity.Name);
            }
            else
            {
                result = _orch.ScoreFormEntry(entryId, Convert.ToInt32(judgeScore), User.Identity.Name);
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
        public ActionResult NewWeaponOrFormScoring(ParticipantSelection participantSelection, long ringId, bool isWeapon)
        {
            OperationResult result;
            try
            {
                var partId = _orch.GetParticipantIdBySelection(participantSelection);

                if (isWeapon)
                {
                    result = _orch.NewWeaponEntry(ringId, partId);
                }
                else
                {
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
            }
            catch (InvalidOperationException ex)
            {
                var exMess = ex.Message;
                AddErrorNotification(Resources.ParticipantNotSelectedMessage);
            }
            catch (Exception ex) {
                AddErrorNotification(ex.Message);
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
            vm.ParticipantSelection = MkParticipantSelection(ringId);
            var svm = mkStationViewModel();
            for (int i = StoredValues.BreakingMaxStationCount; i > 0;--i )
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
            try
            {
                var model = new BreakingResult();
                model.Ring = _orch.GetRingById(vm.RingId);
                model.Participant = _orch.GetParticipantById(_orch.GetParticipantIdBySelection(vm.ParticipantSelection));

                var atLeastOneStation = false;
                foreach (var stationVM in vm.Stations)
                {
                    var result = _orch.CreateTechniqueValue(stationVM.BaseTechniques);
                    if (result.WasSuccessful && result.HasTechniqueValue)
                    {
                        atLeastOneStation = true;
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
                if(!atLeastOneStation){
                    AddErrorNotification(Hyushik_TournMan_Common.Properties.Resources.BreakingTechniqueNotEnteredMessage);
                    return RedirectToRing(vm.RingId);
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
            }
            catch (InvalidOperationException ex)
            {
                var exMess = ex.Message;
                AddErrorNotification(Resources.ParticipantNotSelectedMessage);
            }
            catch (Exception ex) {
                AddErrorNotification(ex.Message);
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
            score.Judge_UserId = _orch.GetUserByName(User.Identity.Name).UserId;

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
