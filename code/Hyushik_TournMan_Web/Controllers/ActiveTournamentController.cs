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
    public class ActiveTournamentController : BaseController
    {
        private IActiveTournamentOrchestrator orch = new ActiveTournamentOrchestrator();

        protected WeaponsOrFormsListingViewModel buildWeaponsViewModel(long tournId){
            var scoringAlgorithm = new WeaponsOrFormsAlgorithm();
            var vm =  new WeaponsOrFormsListingViewModel() 
            {
                Participants = orch.GetParticipantsByTournId(tournId).ToList(),
                IsWeapons = true,
                TournamentId = tournId
            };
            foreach(var result in orch.GetWeaponResultsByTournId(tournId))
            {
                vm.AddListing(result.Participant.Name, scoringAlgorithm.CalculateScore(result), result.Id);
            }

            return vm;


        }

        protected WeaponsOrFormsListingViewModel buildFormsViewModel(long tournId)
        {
            var scoringAlgorithm = new WeaponsOrFormsAlgorithm();
            var vm = new WeaponsOrFormsListingViewModel()
            {
                Participants = orch.GetParticipantsByTournId(tournId).ToList(),
                IsWeapons = false,
                TournamentId = tournId
            };
            foreach (var result in orch.GetFormResultsByTournId(tournId))
            {
                vm.AddListing(result.Participant.Name, scoringAlgorithm.CalculateScore(result), result.Id);
            }

            return vm;
        }

        protected ActiveTournamentViewModel BuildActiveTournamentViewModel(long tournId)
        {
            return new ActiveTournamentViewModel{
                Tournament = orch.GetTournamentById(tournId),
                BreakingScoreListingViewModel = BuildBreakingScoreListingViewModel(tournId),
                WeaponsListingViewModel = buildWeaponsViewModel(tournId),
                FormsListingViewModel = buildFormsViewModel(tournId)
            };
        }

        protected BreakingScoreListingViewModel BuildBreakingScoreListingViewModel(long tournId)
        {
            var vm =  new BreakingScoreListingViewModel
            {
                TournamentId = tournId
            };
            foreach(var entry in orch.GetBreakingResultByTournamentId(tournId)){
                var scoreResult = orch.CalculateBreakingScore(entry);
                if(scoreResult.WasSuccessful){
                    vm.AddListing(entry.Participant.Name, entry.Id, scoreResult.Score, scoreResult.Stations);
                }

                
            }

            return vm;
        }

        public ActionResult Index(long tournId)
        {
            return View(BuildActiveTournamentViewModel(tournId));
        }


    }
}
