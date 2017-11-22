using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using InrapporteringsPortal.Web.Models;
using InrapporteringsPortal.Web.Models.ViewModels;

namespace InrapporteringsPortal.Web.Controllers
{
    public class UploadController : Controller
    {
        private UploadFileViewModel _model = new UploadFileViewModel();


        // GET: Upload
        public ActionResult Index()
        {
            //TODO - används ej?
            return View();
        }


        [HttpGet]
        public ActionResult UploadFile()
        {
            //TODO - sätt currentUser i model?

            // Settings.  
            _model.SelectedRegisterId = 0;

            // Loading drop down lists.  
            //TODO - hämta registerinfo från databasen
            this.ViewBag.RegisterList = this.GetRegisterList();

            return View(_model);
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
                    //TODO - Hämta från config? Jmfr log
                    string nameAndLocation = "C:/Socialstyrelsen/UploadedFiles/EKB/" + _FileName;
                    file.SaveAs(nameAndLocation);
                }
                ViewBag.Message = "Filen/filerna har nu laddats upp!";
                //TODO - Anropa med kommunkod för current user
                AddRegisterListToViewBag(1);
                return View();
            }
            catch
            {
                AddRegisterListToViewBag(1);
                ViewBag.Message = "Uppladdning av filen/filerna misslyckades";
                return View();
            }
        }

        public void AddRegisterListToViewBag(int kommunKod)
        {
            _model.SelectedRegisterId = 0;
            //TODO - hämta för aktuell kommunKod
            ViewBag.RegisterList = this.GetRegisterList();
        }

        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<Register> LoadData()
        {
            //TODO - hämta från databasen
            List<Register> regList = new List<Register>();

            // Initialization.
            Register reg1 = new Register();
            Register reg2 = new Register();
            Register reg3 = new Register();

            // Setting.  
            reg1.Id = 1;
            reg1.Namn = "BU";
            reg2.Id = 2;
            reg2.Namn = "EKB";
            reg3.Id = 3;
            reg3.Namn = "LSS";


            // Adding.  
            regList.Add(reg1);
            regList.Add(reg2);
            regList.Add(reg3);

            // info.  
            return regList;
        }


        /// <summary>  
        /// Get register method.  
        /// </summary>  
        /// <returns>Return register for drop down list.</returns>  
        private IEnumerable<SelectListItem> GetRegisterList()
        {
            // Initialization.  
            SelectList lstobj = null;

                // Loading.  
                var list = LoadData()
                    .Select(p =>
                        new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Namn
                        });

                // Setting.  
                lstobj = new SelectList(list, "Value", "Text");


            // info.  
            return lstobj;
        }
    }
}






   