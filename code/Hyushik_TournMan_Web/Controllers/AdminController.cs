using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyushik_TournMan_BLL.Orchestrators.Interfaces;
using Hyushik_TournMan_BLL.Orchestrators;
using Hyushik_TournMan_Common.Results;
using Hyushik_TournMan_Web.Classes.Constants;
using Hyushik_TournMan_Common.Constants;
using Hyushik_TournMan_Common.Properties;

namespace Hyushik_TournMan_Web.Controllers
{
    public class AdminController : BaseController
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
            OperationResult result;
            if (file != null && file.ContentLength > 0)
            {
                result = orch.importParticipantCsvFile(file.InputStream);
            }
            else
            {
                result =  new OperationResult()
                {
                    WasSuccessful = false,
                    Message = Resources.FileUnreadableMessage
                };
            }


            var resultMessages = new Dictionary<string, string>();
            string notificationClass = String.Empty;
            if (result.WasSuccessful){
                notificationClass = WebConstants.CssClasses.Notifications.SUCCESS;
            }else if(!result.WasSuccessful){
                notificationClass = WebConstants.CssClasses.Notifications.ERROR;
            }

            resultMessages[result.Message] = notificationClass;

            AddNotifications(resultMessages);
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

    }
}
