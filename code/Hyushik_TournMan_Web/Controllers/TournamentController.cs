using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyushik_TournMan_Web.Controllers
{
    public class TournamentController : Controller
    {

        public ActionResult Index(int tournId)
        {
            return View();
        }

    }
}
