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
    public class HomeController : BaseController
    {
        private IHomeOrchestrator orch = new HomeOrchestrator();

        protected HomeViewModel BuildHomeViewModel()
        {
            return new HomeViewModel
            {
                ActivatedTournamentsViewModel = BuildActivatedTournamentsViewModel(), QrViewModel= BuildQrCodeViewModel()
            };
        }
        
        protected ActivatedTournamentsViewModel BuildActivatedTournamentsViewModel()
        {
            return new ActivatedTournamentsViewModel()
            {
                ActiveTournaments = orch.GetActiveTournaments()
            };
        }


        protected QrViewModel BuildQrCodeViewModel()
        {
            return new QrViewModel()
            {
                Base64Qr = orch.getQrcode(1L)
            };
        }



        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(BuildHomeViewModel());
        }
    }
}
