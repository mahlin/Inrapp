using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;  

namespace InrapporteringsPortal.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string pathTmp = Request.PhysicalApplicationPath;
                    //TODO - Hämta från config?
                    string nameAndLocation = "C:/Socialstyrelsen/UploadedFiles/EKB/" + _FileName;
                    file.SaveAs(nameAndLocation);
                    //string _path = Path.Combine(Server.MapPath("~/Socialstyrelsen/UploadedFiles/EKB"), _FileName);
                    //file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}






   