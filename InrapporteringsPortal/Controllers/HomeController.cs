using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;

namespace InrapporteringsPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService = new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        public ActionResult Index()
        {
            ViewBag.Text = _portalService.HamtaInformationsText("Startsida");
            return View();
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