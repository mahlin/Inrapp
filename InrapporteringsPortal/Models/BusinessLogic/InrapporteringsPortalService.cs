using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using InrapporteringsPortal.Models;
using InrapporteringsPortal.Web.Models.DataAccess.Repositories;

namespace InrapporteringsPortal.Web.Models.BusinessLogic
{
    public class InrapporteringsPortalService : IInrapporteringsPortalService
    {
        private readonly IPortalRepository _portalRepository;

        public InrapporteringsPortalService(IPortalRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        //public IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId)
        //{
        //    throw new NotImplementedException();
        //    //var historikLista = new List<FilloggDetaljDTO>();
        //    ////TODO - tidsintervall
        //    //var leveransIdList = _portalRepository.GetLeveransIdnForKommun(kommunId).OrderByDescending(x => x);
        //    //foreach (var id in leveransIdList)
        //    //{
        //    //    var filloggs = _portalRepository.GetFilloggarForLeveransId(id, DateTime.Now, DateTime.Now);
        //    //    foreach (var fillogg in filloggs)
        //    //    {
        //    //        var filloggDetalj = (FilloggDetaljDTO.FromFillogg(fillogg));
        //    //        historikLista.Add(filloggDetalj);
        //    //    }
        //    //}
        //    //return historikLista;
        //}

        public string HamtaKommunKodForAnvandare(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisation(userId);
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }

        public void SparaTillFillogg(string ursprungligFilNamn, string nyttFilNamn, int leveransId)
        {
            _portalRepository.SaveToFilelogg(ursprungligFilNamn, nyttFilNamn, leveransId);
        }

        public int HamtaNyttLeveransId(string userId, int orgId, int registerId, string period)
        {
            //TODO - skicka med regId
            var levId = _portalRepository.GetNewLeveransId(userId, orgId, registerId, period);
            return levId;
        }

        public Organisation GetOrgForEmailDomain(string modelEmail)
        {
            MailAddress address = new MailAddress(modelEmail);
            string domain = address.Host; // host contains yahoo.com
            //var domain = modelEmail.Split('@');
            var organisation = _portalRepository.GetOrgForEmailDomain(domain);
            return organisation;
        }

        public string HamtaKommunKodForOrganisation(int orgId)
        {
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }

        public int GetUserOrganisation(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisation(userId);
            return orgId;
        }
    }
}