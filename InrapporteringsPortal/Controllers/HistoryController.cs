using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.Web.Models;
using InrapporteringsPortal.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace InrapporteringsPortal.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService;


        public HistoryController()
        {
            _portalService =
                new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        }

        [Authorize]
        // GET: History
        public ActionResult Index()
        {
            //Kolla om öppet, annars visa stängt-sida
            if (!_portalService.IsOpen())
            {
                ViewBag.Text = _portalService.HamtaInformationsText("Stangtsida");
                return View("Closed");
            }
            var model = new HistoryViewModels.HistoryViewModel();
            try
            {
                var userOrg = _portalService.HamtaOrgForAnvandare(User.Identity.GetUserId());
                IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForOrganisation(userOrg.Id);
                model.HistorikLista = historyFileList.ToList();
                model.OrganisationsNamn = userOrg.Organisationsnamn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HistoryController", "Index", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade i historiksidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }


            return View(model);
        }
    }
}