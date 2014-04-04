using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Scoring;
using Hyushik_TournMan_Common.Models;
using Hyushik_TournMan_Web.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web.Controllers
{
    public class RingController : BaseController
    {
        //
        // GET: /Ring/

        private IRingOrchestrator orch = new RingOrchestrator();

        protected SparringViewModel mkSparringViewModel(long ringId)
        {
            var partSelect = MkParticipantSelection(ringId);
            return new SparringViewModel()
            {
                SparringResults = orch.GetSparringResultsByRingId(ringId),
                Participant1Selection = partSelect,
                Participant2Selection = partSelect,
                Ring = orch.GetRingById(ringId)
            };
        }
        
        protected ParticipantSelection MkParticipantSelection(long ringId)
        {

            var result = orch.GetParticipantSelectionByRingId(ringId);
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

        protected WeaponsOrFormsListingViewModel buildWeaponsViewModel(long ringId)
        {
            var scoringAlgorithm = new WeaponsOrFormsAlgorithm();
            var vm = new WeaponsOrFormsListingViewModel()
            {
                IsWeapons = true,
                Ring = orch.GetRingById(ringId),
                ParticipantSelection = MkParticipantSelection(ringId)
            };
            foreach (var result in orch.GetWeaponResultsByRingId(ringId))
            {
                vm.AddListing(result.Participant.Name, scoringAlgorithm.CalculateScore(result), result.Id);
            }

            return vm;

        }

        protected WeaponsOrFormsListingViewModel buildFormsViewModel(long ringId)
        {
            var scoringAlgorithm = new WeaponsOrFormsAlgorithm();
            var vm = new WeaponsOrFormsListingViewModel()
            {
                IsWeapons = false,
                Ring = orch.GetRingById(ringId),
                ParticipantSelection = MkParticipantSelection(ringId)
            };
            foreach (var result in orch.GetFormResultsByRingId(ringId))
            {
                vm.AddListing(result.Participant.Name, scoringAlgorithm.CalculateScore(result), result.Id);
            }

            return vm;
        }

        protected RingViewModel BuildRingViewModel(long ringId)
        {
            return new RingViewModel
            {
                Ring = orch.GetRingById(ringId),
                BreakingScoreListingViewModel = BuildBreakingScoreListingViewModel(ringId),
                WeaponsListingViewModel = buildWeaponsViewModel(ringId),
                FormsListingViewModel = buildFormsViewModel(ringId),
                SparringViewModel = mkSparringViewModel(ringId)
            };
        }

        protected BreakingScoreListingViewModel BuildBreakingScoreListingViewModel(long ringId)
        {
            var vm = new BreakingScoreListingViewModel
            {
                Ring = orch.GetRingById(ringId)
            };
            foreach (var entry in orch.GetBreakingResultByRingId(ringId))
            {
                var scoreResult = orch.CalculateBreakingScore(entry);
                var judgeScoresResult = orch.GetBreakingJudgeOpinions(entry.Id);
                if (scoreResult.WasSuccessful && judgeScoresResult.WasSuccessful)
                {
                    vm.AddListing(entry.Participant.Name, entry.Id, scoreResult.Score, scoreResult.Stations, judgeScoresResult.JudgeIdToName, judgeScoresResult.JudgeIdToScore);

                }
                else if (!scoreResult.WasSuccessful)
                {
                    AddErrorNotification(scoreResult.Message);
                }
                else if (!judgeScoresResult.WasSuccessful)
                {
                    AddErrorNotification(judgeScoresResult.Message);
                }


            }

            return vm;
        }

        public ActionResult Index(long ringId)
        {
            return View(BuildRingViewModel(ringId));
        }

        [HttpPost]
        public ActionResult SetBreakingPublic(long ringId, bool status)
        {
            var result = orch.SetRingBreakingResultsPublicStatus(ringId, status);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else
            {
                AddErrorNotification(result.Message);
            }
            
            return RedirectToAction("Index",new {ringId=ringId});
        }

        [HttpPost]
        public ActionResult SetWeaponsPublic(long ringId, bool status)
        {
            var result = orch.SetRingWeaponResultsPublicStatus(ringId, status);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else
            {
                AddErrorNotification(result.Message);
            }

            return RedirectToAction("Index", new { ringId = ringId });
        }

        [HttpPost]
        public ActionResult SetFormsPublic(long ringId, bool status)
        {
            var result = orch.SetRingFormResultsPublicStatus(ringId, status);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else
            {
                AddErrorNotification(result.Message);
            }

            return RedirectToAction("Index", new { ringId = ringId });
        }

        [HttpPost]
        public ActionResult SetSparringPublic(long ringId, bool status)
        {
            var result = orch.SetRingSparringResultsPublicStatus(ringId, status);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else
            {
                AddErrorNotification(result.Message);
            }

            return RedirectToAction("Index", new { ringId = ringId });
        }

    }
}
