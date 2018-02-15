using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Configuration;
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

        [Authorize] 
        public ActionResult Index()
        {
            try
            {
                var userOrg = _portalService.HamtaOrgForAnvandare(User.Identity.GetUserId());
                //Hämta info om valbara register
                var registerInfoList = _portalService.HamtaValdaRegistersForAnvandare(User.Identity.GetUserId(), userOrg.Id);
                _model.RegisterList = registerInfoList.ToList();

                // Ladda drop down lists.  
                this.ViewBag.RegisterList = CreateRegisterDropDownList(registerInfoList);
                _model.SelectedRegisterId = "0";

                //Hämta historiken för användarens organisation/kommun
                var userId = User.Identity.GetUserId();
                
                //var kommunKodForUser = _portalService.HamtaKommunKodForAnvandare(userId);
                //var orgIdForUser = _portalService.HamtaUserOrganisationId(userId);

                //TODO - ny databas - hämta från Organisationstabellen istället
                var kommunKodForUser = userOrg.Kommunkod;
                var orgIdForUser = userOrg.Id;

                _model.GiltigKommunKod = kommunKodForUser;
                _model.OrganisationsNamn = userOrg.Organisationsnamn;
                IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForOrganisation(orgIdForUser);
                _model.HistorikLista = historyFileList.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("FileUploadController", "Index", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade i filuppladdningssidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);
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
        [Authorize]
        public JsonResult Upload(FilesViewModel model)
        {
            var enhetskod = String.Empty;
            //TODO - test
            //model.SelectedUnitId = "S";
            var resultList = new List<ViewDataUploadFilesResult>();
            var userName = "";
            try
            {
                var kommunKod = _portalService.HamtaKommunKodForAnvandare(User.Identity.GetUserId());
                userName = User.Identity.GetUserName();
                var CurrentContext = HttpContext;
                
                if (model.SelectedUnitId != null)
                    enhetskod = model.SelectedUnitId;

                filesHelper.UploadAndShowResults(CurrentContext, resultList, User.Identity.GetUserId(), userName,
                    kommunKod, Convert.ToInt32(model.SelectedRegisterId), enhetskod, model.RegisterList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("FileUploadController", "Upload", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid uppladdning av fil.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                RedirectToAction("CustomError", new { model = errorModel });
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
                        ErrorManager.WriteToErrorLog("FileUploadController", "Upload", e.ToString(), e.HResult, User.Identity.Name);
                        var errorModel = new CustomErrorPageModel
                        {
                            Information = "Ett fel inträffade när filen skulle sparas till registrets logg.",
                            ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                            ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                        };
                        RedirectToAction("CustomError", new { model = errorModel});
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


        public ActionResult DownloadFile(string filename)
        {
            try
            {
                var dir = WebConfigurationManager.AppSettings["DirForFeedback"];
                string filepath = dir + filename;
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                string contentType = MimeMapping.GetMimeMapping(filepath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                Response.Headers.Add("Content-Disposition", cd.ToString());

                var a =  File(filedata, contentType);
                //View file
                var x =  File(filedata, MediaTypeNames.Text.Plain);
                //Download file
                var y =  File(filedata, MediaTypeNames.Text.Plain, "Test.txt");

                //Öppnar excel
                return File(filedata, contentType);

                    //Funkar ej
                    //return File(filedata, MediaTypeNames.Text.Plain, "Test.txt");

                    //Öppnar filen as is
                    //return File(filedata, MediaTypeNames.Text.Plain);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("FileUploadController", "DownloadFile", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade vid öppningen av återkopplingsfilen",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                    ContactPhonenumber = ConfigurationManager.AppSettings["ContactPhonenumber"]
                };
                return View("CustomError", errorModel);

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

        //public IEnumerable<RegisterInfo> GetRegisterInfo()
        //{
        //    var allaRegisterList= _portalService.HamtaAllRegisterInformation();

        //    //Visa bara de register som användaren valt att repportera till
        //    var chosenRegisters = _portalService.HamtaValdaRegistersForAnvandare(User.Identity.GetUserId());

        //    foreach (var register in allaRegisterList)
        //    {
        //        foreach (var VARIABLE in COLLECTION)
        //        {
                    
        //        }
        //    }

            
        //    return registerList;
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

        public ActionResult CustomError(CustomErrorPageModel model)
        {
            return View(model);
        }
    }
}