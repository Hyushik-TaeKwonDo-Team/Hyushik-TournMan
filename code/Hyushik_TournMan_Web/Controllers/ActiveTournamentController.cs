using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
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

        protected ActiveTournamentViewModel BuildActiveTournamentViewModel(long tournId)
        {
            return new ActiveTournamentViewModel{
                Tournament = orch.GetTournamentById(tournId),
                BreakingScoreListingViewModel = BuildBreakingScoreListingViewModel(tournId)
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
                    vm.AddListing(entry.Participant.Name, entry.Id, scoreResult.Score);
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
