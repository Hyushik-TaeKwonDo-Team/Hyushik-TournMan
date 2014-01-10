using Hyushik_TournMan_Web.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_Common.Extensions;
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


            return vm;
        }

        private BreakingViewModel mkBreakingViewModel()
        {
            var vm = new BreakingViewModel();
            var svm = mkStationViewModel();
            for (int i = 5; i > 0;--i )
            {
                vm.Stations.Add( svm.DeepClone<StationViewModel>() );
            }
            return vm;
        }


        public ActionResult CreateBreakingEntry()
        {
            BreakingViewModel vm = mkBreakingViewModel();

            return View(vm);
        }

        [HttpPost]
        public ActionResult SumbitBreakingEntry(BreakingViewModel vm)
        {
            return View();
        }

    }
}
