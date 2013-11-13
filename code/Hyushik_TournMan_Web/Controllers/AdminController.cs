﻿using System;
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
using Hyushik_TournMan_Web.Classes.ViewModels;

namespace Hyushik_TournMan_Web.Controllers
{
    public class AdminController : BaseController
    {
        private IAdminOrchestrator orch = new AdminOrchestrator();

        //
        // GET: /Admin/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {


            return View(_buildAdminVM());
        }

        private ImportCsvViewModel _buildImportCsvVM()
        {
            var vm = new ImportCsvViewModel()
            {
                Tournaments=orch.GetAllTournaments()
            };
            return vm;
        }

        private TournamentsViewModel _buildTournamentsVM()
        {
            var vm = new TournamentsViewModel()
            {
                Tournaments = orch.GetAllTournaments()
            };
            return vm;
        }

        private AdminViewModel _buildAdminVM()
        {
            var vm = new AdminViewModel()
            {
                ImportCsvViewModel = _buildImportCsvVM(),
                TournamentViewModel = _buildTournamentsVM()
            };
            return vm;
        }

        [HttpPost]
        public ActionResult ImportCsv(ImportCsvViewModel vm)
        {
            // Verify that the user selected a file
            OperationResult result;
            var file = vm.CsvFile;
            if (file != null && file.ContentLength > 0)
            {
                result = orch.ImportParticipantCsvFile(file.InputStream, vm.SelectedTournamentId);
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
                AddSucessNotification(result.Message);
            }else if(!result.WasSuccessful){
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Tournament(long id)
        {
            var vm = new TournamentViewModel()
            {
                Tournament = orch.GetTournamentById(id),
                TotalBoardCounts = orch.GetTotalBoardSizeCountsByTournamentId(id)

            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddTournament(TournamentsViewModel vm)
        {
            var result = orch.CreateNewTournament(vm.NewTournamentName);

            var resultMessages = new Dictionary<string, string>();
            string notificationClass = String.Empty;
            if (result.WasSuccessful)
            {
                AddSucessNotification(result.Message);
            }
            else if (!result.WasSuccessful)
            {
                AddErrorNotification(result.Message);
            }
            return RedirectToAction("Index");
        }

    }
}
