using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.Web.Models;

namespace InrapporteringsPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService = new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        public ActionResult Index()
        {
            try
            {
                ViewBag.Text = _portalService.HamtaInformationsText("Startsida");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HomeController", "Index", e.ToString(), e.HResult);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid öppning av startsidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Information här";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakta oss";

            return View();
        }
    }
}