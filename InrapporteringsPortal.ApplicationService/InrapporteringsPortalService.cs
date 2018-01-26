﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Interface;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.ApplicationService
{
    public class InrapporteringsPortalService : IInrapporteringsPortalService
    {
        private readonly IPortalRepository _portalRepository;

        public InrapporteringsPortalService(IPortalRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId)
        {
            throw new NotImplementedException();
            //var historikLista = new List<FilloggDetaljDTO>();
            ////TODO - tidsintervall
            //var leveransIdList = _portalRepository.GetLeveransIdnForKommun(kommunId).OrderByDescending(x => x);
            //foreach (var id in leveransIdList)
            //{
            //    var filloggs = _portalRepository.GetFilloggarForLeveransId(id, DateTime.Now, DateTime.Now);
            //    foreach (var fillogg in filloggs)
            //    {
            //        var filloggDetalj = (FilloggDetaljDTO.FromFillogg(fillogg));
            //        historikLista.Add(filloggDetalj);
            //    }
            //}
            //return historikLista;
        }

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId)
        {
            var historikLista = new List<FilloggDetaljDTO>();
            //TODO - tidsintervall?
            //var leveransIdList = _portalRepository.GetLeveransIdnForOrganisation(orgId).OrderByDescending(x => x);
            var leveransList = _portalRepository.GetLeveranserForOrganisation(orgId);
            foreach (var leverans in leveransList)
            {
                var filloggDetalj = new FilloggDetaljDTO();
                //Kolla om återkopplingsfil finns för aktuell leverans
                var aterkoppling = _portalRepository.GetAterkopplingForLeverans(leverans.Id);

                var filer = _portalRepository.GetFilerForLeveransId(leverans.Id);
                var registerKortnamn = _portalRepository.GetRegisterKortnamn(leverans.DelregisterId);
                foreach (var fil in filer)
                {
                    filloggDetalj = (FilloggDetaljDTO.FromFillogg(fil));
                    filloggDetalj.Kontaktperson = leverans.ApplicationUserId;
                    filloggDetalj.Leveransstatus = leverans.Leveransstatus;
                    filloggDetalj.Leveranstidpunkt = leverans.Leveranstidpunkt;
                    filloggDetalj.RegisterKortnamn = registerKortnamn;
                    filloggDetalj.Resultatfil = "  -  ";
                    historikLista.Add(filloggDetalj);

                    if (aterkoppling != null)
                    {
                        filloggDetalj.Leveransstatus = aterkoppling.Leveransstatus;
                        filloggDetalj.Resultatfil = aterkoppling.Resultatfil;
                    }
                }
            }
            var sorteradHistorikLista = historikLista.OrderByDescending(x => x.Leveranstidpunkt).ToList();

            return sorteradHistorikLista;
        }

        public string HamtaKommunKodForAnvandare(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisationId(userId);
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }
        
        public void SparaTillDatabasFillogg(string userName, string ursprungligFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber)
        {
            _portalRepository.SaveToFilelogg(userName, ursprungligFilNamn, nyttFilNamn, leveransId, sequenceNumber);
        }

        public int HamtaNyttLeveransId(string userId, string userName, int orgId, int registerId, int forvLevId)
        {
            var levId = _portalRepository.GetNewLeveransId(userId, userName, orgId, registerId, forvLevId);
            return levId;
        }

        public Organisation HamtaOrgForEmailDomain(string modelEmail)
        {
            MailAddress address = new MailAddress(modelEmail);
            string domain = address.Host; 
            var organisation= _portalRepository.GetOrgForEmailDomain(domain);
            return organisation;
        }

        public string HamtaKommunKodForOrganisation(int orgId)
        {
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }

        public int HamtaUserOrganisationId(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisationId(userId);
            return orgId;
        }

        public IEnumerable<RegisterInfo> HamtaAllRegisterInformation()
        {
            var registerList = _portalRepository.GetAllRegisterInformation();
            return registerList;
        }

        public void SaveToLoginLog(string userid)
        {
            _portalRepository.SaveToLoginLog(userid);
        }

        public string HamtaInformationsText(string infoTyp)
        {
            var infoText = _portalRepository.GetInformationText(infoTyp);
            return infoText;
        }

        public Organisation HamtaOrgForAnvandare(string userId)
        {
            var org = _portalRepository.GetOrgForUser(userId);
            return org;
        }
    }
}