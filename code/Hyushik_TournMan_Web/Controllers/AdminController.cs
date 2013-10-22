using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Orchestrators;

namespace Hyushik_TournMan_Web.Controllers
{
    public class AdminController : Controller
    {
        private ICsvImportOrchestrator orch = new CsvImportOrchestrator();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportCsv(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                orch.importParticipantCsvFile(file.InputStream);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

    }
}
