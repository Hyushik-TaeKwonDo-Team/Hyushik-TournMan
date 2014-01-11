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

        private BreakingViewModel mkBreakingViewModel()
        {
            var vm = new BreakingViewModel();
            var svm = mkStationViewModel();
            for (int i = 1; i > 0;--i )
            {
                //tried to limit this with a deepcopy, it didn't work . . .
                vm.Stations.Add( mkStationViewModel() );
            }
            return vm;
        }


        public ActionResult CreateBreakingEntry()
        {
            BreakingViewModel vm = mkBreakingViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateBreakingEntry(BreakingViewModel vm)
        {
            return RedirectToAction("CreateBreakingEntry");
        }

    }
}
