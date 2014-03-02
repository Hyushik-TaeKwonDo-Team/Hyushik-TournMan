using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Scoring;
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

        protected WeaponsOrFormsListingViewModel buildWeaponsViewModel(long ringId)
        {
            var scoringAlgorithm = new WeaponsOrFormsAlgorithm();
            var vm = new WeaponsOrFormsListingViewModel()
            {
                Participants = orch.GetParticipantsByRingId(ringId).ToList(),
                IsWeapons = true,
                TournamentId = ringId
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
                Participants = orch.GetParticipantsByRingId(ringId).ToList(),
                IsWeapons = false,
                TournamentId = ringId
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
                Tournament = orch.GetTournamentById(ringId),
                BreakingScoreListingViewModel = BuildBreakingScoreListingViewModel(ringId),
                WeaponsListingViewModel = buildWeaponsViewModel(ringId),
                FormsListingViewModel = buildFormsViewModel(ringId)
            };
        }

        protected BreakingScoreListingViewModel BuildBreakingScoreListingViewModel(long ringId)
        {
            var vm = new BreakingScoreListingViewModel
            {
                TournamentId = ringId
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

    }
}
