﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InrapporteringsPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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