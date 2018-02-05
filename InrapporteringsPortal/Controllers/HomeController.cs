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
        private readonly IInrapporteringsPortalService _portalService = new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        public ActionResult Index()
        {
            try
            {
                //Kolla om öppet, annars visa stängt-sida
                if (IsOpen())
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
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
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
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakta oss";

            return View();
        }

        private bool IsOpen()
        {
            var open = true;

            var now = DateTime.Now;
            var today = now.DayOfWeek.ToString();
            var hourNow = now.Hour;
            var minuteNow = now.Minute;

            //var closedDays = WebConfigurationManager.AppSettings["ClosedDays"];
            var closedDays = new List<string>(WebConfigurationManager.AppSettings["ClosedDays"].Split(new char[] { ',' }));

            var closedFromHour = Convert.ToInt32(WebConfigurationManager.AppSettings["ClosedFromHour"]);
            var closedFromMin = Convert.ToInt32(WebConfigurationManager.AppSettings["ClosedFromMin"]);
            var closedToHour = Convert.ToInt32(WebConfigurationManager.AppSettings["ClosedToHour"]);
            var closedToMin = Convert.ToInt32(WebConfigurationManager.AppSettings["ClosedToMin"]);
            var closedAnyway = Convert.ToBoolean(WebConfigurationManager.AppSettings["ClosedAnyway"]);


            //Test
            //hourNow = 8;
            //minuteNow = 2;
            
            if (closedAnyway)
            {
                return false;
            }

            //Check day
            foreach (var day in closedDays)
            {
                if (day == today)
                {
                    return false;
                }
            }

            //Check time
            //if ((closedFromHour > hourNow) && (hourNow > closedToHour))
            //{
            //    return true;
            //}

            //After closing
            if ((closedFromHour <= hourNow))
            {
                //Check minute
                if (minuteNow < closedFromMin)
                {
                    return true;
                }
                return false;
            }

            //Before opening?
            if ((closedToHour <= hourNow))
            {
                //Check minute
                if (minuteNow > closedToMin)
                {
                    return true;
                }
                return false;
            }


            return true;
        }
    }
}