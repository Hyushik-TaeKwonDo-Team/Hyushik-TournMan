﻿using Hyushik_TournMan_BLL.Orchestrators;
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
                Tournament = orch.GetTournamentById(tournId)
            };
        }

        public ActionResult Index(long tournId)
        {
            return View(BuildActiveTournamentViewModel(tournId));
        }

    }
}
