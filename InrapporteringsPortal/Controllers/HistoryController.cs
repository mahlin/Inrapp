using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.ApplicationService;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.ApplicationService.Interface;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;
using InrapporteringsPortal.Web.Models;
using InrapporteringsPortal.Web.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace InrapporteringsPortal.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IInrapporteringsPortalService _portalService;


        public HistoryController()
        {
            _portalService =
                new InrapporteringsPortalService(new PortalRepository(new ApplicationDbContext()));

        }

        [Authorize]
        // GET: History
        public ActionResult Index()
        {
            //Kolla om öppet, annars visa stängt-sida
            if (!_portalService.IsOpen())
            {
                ViewBag.Text = _portalService.HamtaInformationsText("Stangtsida");
                return View("Closed");
            }
            var model = new HistoryViewModels.HistoryViewModel();
            try
            {
                var userId = User.Identity.GetUserId();
                var userOrg = _portalService.HamtaOrgForAnvandare(User.Identity.GetUserId());
                IEnumerable<FilloggDetaljDTO> historyFileList = _portalService.HamtaHistorikForOrganisation(userOrg.Id);
                model.HistorikLista = historyFileList.ToList();
                model.OrganisationsNamn = userOrg.Organisationsnamn;
                IEnumerable<AdmRegister> admRegList = _portalService.HamtaRegisterForAnvandare(userId, userOrg.Id);
                model.RegisterList = ConvertAdmRegisterToViewModel(admRegList.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorManager.WriteToErrorLog("HistoryController", "Index", e.ToString(), e.HResult, User.Identity.Name);
                var errorModel = new CustomErrorPageModel
                {
                    Information = "Ett fel inträffade i historiksidan.",
                    ContactEmail = ConfigurationManager.AppSettings["ContactEmail"],
                };
                return View("CustomError", errorModel);
            }


            return View(model);
        }

        private List<HistoryViewModels.AdmRegisterViewModel> ConvertAdmRegisterToViewModel(List<AdmRegister> admRegisterList)
        {
            var registerList = new List<HistoryViewModels.AdmRegisterViewModel>();
            foreach (var register in admRegisterList)
            {
                var delregisterList = new List<HistoryViewModels.AdmDelregisterViewModel>();
                var registerView = new HistoryViewModels.AdmRegisterViewModel()
                {
                    Id = register.Id,
                    Registernamn = register.Registernamn,
                    Beskrivning = register.Beskrivning,
                    Kortnamn = register.Kortnamn
                };

                foreach (var delregister in register.AdmDelregister)
                {
                    var delregisterView = new HistoryViewModels.AdmDelregisterViewModel()
                    {
                        Id = delregister.Id,
                        RegisterId = delregister.RegisterId,
                        Delregisternamn = delregister.Delregisternamn,
                        Kortnamn = delregister.Kortnamn,
                        Beskrivning = delregister.Beskrivning
                    };
                    delregisterList.Add(delregisterView);

                }
                registerView.DelRegister = delregisterList.ToList();
                registerList.Add(registerView);
            }
            return registerList;
        }
    }
}