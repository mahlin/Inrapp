using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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
        private readonly IInrapporteringsPortalService _portalService =
            new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        public ActionResult Index()
        {
            try
            {
                //Kolla om öppet, annars visa stängt-sida
                if (_portalService.IsOpen())
                {
                    ViewBag.Text = _portalService.HamtaInformationsText("Startsida");
                    return View();
                }
                else
                {
                    ViewBag.Text = _portalService.HamtaInformationsText("Stangtsida");
                    return View("Closed");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HomeController", "Index", e.ToString(), e.HResult);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid öppning av startsidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
        }

        public ActionResult About()
        {
            try
            {
                ViewBag.Text = _portalService.HamtaInformationsText("Hjalpsida");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HomeController", "About", e.ToString(), e.HResult);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid öppning av hjälpsidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
        }

        public ActionResult Contact()
        {
            try
            {
                ViewBag.Text = _portalService.HamtaInformationsText("Kontaktsida");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HomeController", "Contact", e.ToString(), e.HResult);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid öppning av kontaktsidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }
        }
    }
}
