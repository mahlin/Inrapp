﻿using System;
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
using InrapporteringsPortal.Web.Models.ViewModels;

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
                //Kolla om avvikande öppettider finns inom den kommande veckan
                var str = _portalService.ClosedComingWeek();
                if (str != String.Empty)
                    ViewBag.AvvikandeOppettider = "Avvikande öppettider<br/>" + str;
                    //"Avvikande öppettider<br/>Måndag 20 augusti 10 - 16 Underhåll <br/>Måndag 24 december stängt Julafton <br/> ";

                    //Kolla om öppet, annars visa stängt-sida
                if (_portalService.IsOpen())
                {
                    ViewBag.Text = _portalService.HamtaInformationsText("Startsida");

                    return View();
                }
                else
                {
                    if (_portalService.IsHelgdag() || _portalService.IsSpecialdag())
                    {
                        ViewBag.Text = _portalService.HamtaHelgEllerSpecialdagsInfo();
                    }
                    else
                    {
                        ViewBag.Text = _portalService.HamtaInformationsText("Stangtsida");
                    }
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

        public ActionResult About( bool closed = false)
        {
            try
            {
                //Hamta FAQs
                var model = new AboutViewModel();
                model.FaqCategories = _portalService.HamtaFAQs();
                model.PortalClosed = closed;
                ViewBag.Text = _portalService.HamtaInformationsText("Hjalpsida");
                return View(model);
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

        public ActionResult Contact(bool closed = false)
        {
            try
            {
                var model = new AboutViewModel();
                model.PortalClosed = closed;
                ViewBag.Text = _portalService.HamtaInformationsText("Kontaktsida");
                return View(model);
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
