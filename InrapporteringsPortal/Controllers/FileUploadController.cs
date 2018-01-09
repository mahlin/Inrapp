using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.Web.Models;
using InrapporteringsPortal.Web.Models.ViewModels;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;
using Microsoft.AspNet.Identity;

namespace InrapporteringsPortal.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService;
        private FilesViewModel _model = new FilesViewModel();
        FilesHelper filesHelper;
        //String tempPath = "~/somefiles/";
        //String serverMapPath = "~/UppladdadeRegisterfiler/";

        private string StorageRoot
        {
            //get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
            get { return Path.Combine(ConfigurationManager.AppSettings["uploadFolder"]); }
           
        }

        //private string UrlBase = "/Files/somefiles/";
        //String DeleteURL = "/FileUpload/DeleteFile/?file=";
        //String DeleteType = "GET";

        public FileUploadController()
        {
            filesHelper = new FilesHelper(StorageRoot);
            //filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);

            _portalService =
                new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));
        }

        //[Authorize] 
        public ActionResult Index()
        {
            //Hämta info om valbara register
            var registerInfoList = GetRegisterInfo().ToList();
            _model.RegisterList = registerInfoList;

            // Ladda drop down lists.  
            this.ViewBag.RegisterList = CreateRegisterDropDownList(registerInfoList);
            _model.SelectedRegisterId = "0";

            //TODO - hämta kommunId från current user
            //Hämta historiken för användarens kommun
            try
            {
                var userId = User.Identity.GetUserId();
                var kommunKodForUser = _portalService.HamtaKommunKodForAnvandare(userId);
                var orgIdForUser = _portalService.GetUserOrganisation(userId);
                _model.GiltigKommunKod = kommunKodForUser;
                IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForOrganisation(orgIdForUser);
                _model.HistorikLista = historyFileList.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("FileUploaderController", "Index, Hämta historik", e.ToString());
            }

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
        public JsonResult Upload(FilesViewModel model)
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            
            var kommunKod = _portalService.HamtaKommunKodForAnvandare(User.Identity.GetUserId());
            var userName = User.Identity.GetUserName();

            var CurrentContext = HttpContext;

            try
            {
                filesHelper.UploadAndShowResults(CurrentContext, resultList, User.Identity.GetUserId(), userName, kommunKod, Convert.ToInt32(model.SelectedRegisterId), model.RegisterList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("FileUploaderController", "Upload", e.ToString());
            }


            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                //Save to database filelog
                foreach (var itemFile in resultList)
                {
                    try
                    {
                        _portalService.SparaTillDatabasFillogg(userName, itemFile.name, itemFile.sosName, itemFile.leveransId, itemFile.sequenceNumber);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        ErrorManager.WriteToErrorLog("FileUploaderController", "Upload, SaveToFileLog", e.ToString());
                    }
                }
                //TODO - refresh Historiklistan, annan model
                //var userId = User.Identity.GetUserId();
                //var orgIdForUser = _portalService.GetUserOrganisation(userId);
                //IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForOrganisation(orgIdForUser);
                //model.HistorikLista = historyFileList.ToList();
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
            _model.SelectedRegisterId = "0";
            //TODO - hämta för aktuell kommunKod
            ViewBag.RegisterList = CreateRegisterDropDownList(_model.RegisterList);
        }

        private IEnumerable<RegisterInfo> GetRegisterInfo()
        {
            var registerList= _portalService.GetAllRegisterInformation();

            //TODO - antal filer ska finnas att hämta i databasen "admForvantadfil"? Tills dess hårdkodat
            //foreach (var item in registerList)
            //{
            //    if (item.Kortnamn == "BU")
            //    {
            //        item.AntalFiler = 2;
            //    }
            //    else
            //    {
            //        item.AntalFiler = 1;
            //    }
            //}

            //var reg1 = new RegisterInfo();
            //var reg2 = new RegisterInfo();
            //var reg3 = new RegisterInfo();
            //var str1 = "Filnamn ska vara i formatet BU_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Bu_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 2";
            //var str2 = "Filnamn ska vara i formatet EBK_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Ekb_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 1";
            //var str3 = "Filnamn ska vara i formatet LSS_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Lss_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 1";
            //var registerInfoTextList = new List<KeyValuePair<int, string>>();
            //registerInfoTextList.Add(new KeyValuePair<int, string>(1, str1));
            //registerInfoTextList.Add(new KeyValuePair<int, string>(2, str2));
            //registerInfoTextList.Add(new KeyValuePair<int, string>(3, str3));
            //_model.RegisterInfoText = registerInfoTextList;

            //var registerFilAntalList = new List<KeyValuePair<int, int>>();
            //registerFilAntalList.Add(new KeyValuePair<int, int>(1, 2));
            //registerFilAntalList.Add(new KeyValuePair<int, int>(2, 1));
            //registerFilAntalList.Add(new KeyValuePair<int, int>(3, 1));
            //_model.FilAntal = registerFilAntalList;

            //var registerFilmaskList = new List<KeyValuePair<int, string>>();
            //registerFilmaskList.Add(new KeyValuePair<int, string>(1, "^BU_INSATS_\\d{4}_20\\d{2}_20\\d{2}(01|02|03|04|05|06|07|08|09|10|11|12)(0|1|2|3)\\dT(0|1|2)\\d(0|1|2|3|4|5)\\d.TXT$"));
            //registerFilmaskList.Add(new KeyValuePair<int, string>(2, "^EKB_\\d{4}_20\\d{2}(01|02|03|04|05|06|07|08|09|10|11|12)_20\\d{2}(01|02|03|04|05|06|07|08|09|10|11|12)(0|1|2|3)\\dT(0|1|2)\\d(0|1|2|3|4|5)\\d.TXT$"));
            //registerFilmaskList.Add(new KeyValuePair<int, string>(3, "{c.*g}"));
            //_model.FilMask = registerFilmaskList;

            //var registerList = new List<RegisterInfo>();
            //reg1.Namn = "Barn och ungdom";
            //reg1.Id = 1;
            //reg1.InfoText = str1;
            //reg1.Kortnamn = "BU";
            //reg1.AntalFiler = 2;
            //reg1.FilMask = "^[A-Za-z]+$";

            //reg2.Namn = "Ekonomiskt bistånd";
            //reg2.Id = 2;
            //reg2.InfoText = str2;
            //reg2.Kortnamn = "EKB";
            //reg2.AntalFiler = 1;
            //reg2.FilMask = "^Ekb_\\d{4}_20\\d{2}(01|02|03|04|05|06|07|08|09|10|11|12)_20\\d{2}(01|02|03|04|05|06|07|08|09|10|11|12)(0|1|2|3)\\d(0|1|2)\\d(0|1|2|3|4|5)\\d.TXT$";

            //reg3.Namn = "Lagen om stöd och service";
            //reg3.Id = 3;
            //reg3.InfoText = str3;
            //reg3.Kortnamn = "LSS";
            //reg3.AntalFiler = 1;
            //reg3.FilMask = "{c.*g}";

            //registerList.Add(reg1);
            //registerList.Add(reg2);
            //registerList.Add( reg3);

            //_model.RegisterList = registerList;

            return registerList;
        }

        //private List<KeyValuePair<int, string>> GetRegisterInfoTexts()
        //{
        //    //TODO - Get registers information texts from db
        //    var str1 = "Filnamn ska vara i formatet BU_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Bu_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 2";
        //    var str2 = "Filnamn ska vara i formatet EBK_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Ekb_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 1";
        //    var str3 = "Filnamn ska vara i formatet LSS_Länskod och Kommunkod_Inrapporteringsperiod_datum och klockslag.txt<br/>Ex.Lss_1122_1710_1711141532.txt <br/>Antal filer som ska inrapporteras i en leverans: 1";
        //    var registerInfoTextList = new List<KeyValuePair<int, string>>();
        //    registerInfoTextList.Add(new KeyValuePair<int, string>(1, str1));
        //    registerInfoTextList.Add(new KeyValuePair<int, string>(2, str2));
        //    registerInfoTextList.Add(new KeyValuePair<int, string>(3, str3));

        //    return registerInfoTextList;
        //}

        /// <summary>  
        /// Create list for register-dropdown  
        /// </summary>  
        /// <returns>Return register for drop down list.</returns>  
        private IEnumerable<SelectListItem> CreateRegisterDropDownList(IEnumerable<RegisterInfo> registerInfoList)
        {
            SelectList lstobj = null;

            var list = registerInfoList
                .Select(p =>
                    new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Namn
                    });

            // Setting.  
            lstobj = new SelectList(list, "Value", "Text");

            return lstobj;
        }

        ///// <summary>  
        ///// Load data method.  
        ///// </summary>  
        ///// <returns>Returns - Data</returns>  
        //private List<Register> LoadData()
        //{
        //    //TODO - hämta från databasen
        //    List<Register> regList = new List<Register>();

        //    // Initialization.
        //    Register reg1 = new Register();
        //    Register reg2 = new Register();
        //    Register reg3 = new Register();

        //    // Setting.  
        //    reg1.Id = 1;
        //    reg1.Namn = "BU";
        //    reg2.Id = 2;
        //    reg2.Namn = "EKB";
        //    reg3.Id = 3;
        //    reg3.Namn = "LSS";


        //    // Adding.  
        //    regList.Add(reg1);
        //    regList.Add(reg2);
        //    regList.Add(reg3);

        //    // info.  
        //    return regList;
        //}


    }
}