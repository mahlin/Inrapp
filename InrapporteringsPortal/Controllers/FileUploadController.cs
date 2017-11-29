using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.Web.Helpers;
using InrapporteringsPortal.Web.Models;
using InrapporteringsPortal.Web.Models.ViewModels;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;

namespace InrapporteringsPortal.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService;
        private FilesViewModel _model = new FilesViewModel();
        FilesHelper filesHelper;
        String tempPath = "~/somefiles/";
        String serverMapPath = "~/Files/somefiles/";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        private string UrlBase = "/Files/somefiles/";
        String DeleteURL = "/FileUpload/DeleteFile/?file=";
        String DeleteType = "GET";
        public FileUploadController()
        {
           filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
            _portalService = new InrapporteringsPortalService(new PortalRepository(new InrapporteringsPortalDbContext()));
        }


        public ActionResult Index()
        {
            //TODO - sätt currentUser i model?
            _model.RegisterInfoText = GetRegisterInfoTexts();
            _model.SelectedRegisterId = 0;

            // Ladda drop down lists.  
            //TODO - hämta registerinfo från databasen
            this.ViewBag.RegisterList = this.GetRegisterList();

            //Hämta historiken för användarens kommun
            IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForKommun(3333);

            _model.HistorikLista = historyFileList.ToList();

            return View(_model);
        }
        public ActionResult Show()
        {
            JsonFiles ListOfFiles = filesHelper.GetFileList();
            //IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForKommun(1);

            var model = new FilesViewModel()
            {
                Files = ListOfFiles.files,
                //HistorikLista = historyFileList.ToList()
            };
          
            return View(model);
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
           
            var CurrentContext = HttpContext;

            filesHelper.UploadAndShowResults(CurrentContext, resultList);
            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
            }
        }
        public JsonResult GetFileList()
        {
            var list=filesHelper.GetFileList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteFile(string file)
        {
            filesHelper.DeleteFile(file);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public void AddRegisterListToViewBag(int kommunKod)
        {
            _model.SelectedRegisterId = 0;
            //TODO - hämta för aktuell kommunKod
            ViewBag.RegisterList = this.GetRegisterList();
        }

        private List<KeyValuePair<int, string>> GetRegisterInfoTexts()
        {
            //TODO - Get registers information texts from db
            var str1 = "Filnamn ska vara i formatet BU_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Bu_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 1";
            var str2 = "Filnamn ska vara i formatet EBK_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Ekb_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 2";
            var str3 = "Filnamn ska vara i formatet LSS_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Lss_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 3";
            var registerInfoTextList = new List<KeyValuePair<int, string>>();
            registerInfoTextList.Add(new KeyValuePair<int, string>(1, str1));
            registerInfoTextList.Add(new KeyValuePair<int, string>(2, str2));
            registerInfoTextList.Add(new KeyValuePair<int, string>(3, str3));

            return registerInfoTextList;
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


    }
}