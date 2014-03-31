using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Scoring;
using Hyushik_TournMan_Common.Constants;
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

        IActiveTournamentOrchestrator _orch = new ActiveTournamentOrchestrator();

        public ActionResult Index(long tournId)
        {
            var tourn = _orch.GetTournamentById(tournId);
            return View(tourn);
        }

        [Authorize(Roles = Constants.Roles.JUDGE_ROLE)]
        public ActionResult RingCheckIn(long tournId)
        {
            var tourn = _orch.GetTournamentById(tournId);
            return View(tourn);
        }

        [Authorize(Roles = Constants.Roles.JUDGE_ROLE)]
        [HttpPost]
        public ActionResult RingCheckIn(long partId, long ringId, long tournId)
        {
            var result = _orch.CheckInParticipantToRing(partId, ringId);
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("RingCheckIn", new { tournId=tournId});
        }
    }
}
