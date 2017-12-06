using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var historikLista = new List<FilloggDetaljDTO>();
            //TODO - tidsintervall
            var leveransIdList = _portalRepository.GetLeveransIdnForKommun(kommunId).OrderByDescending(x => x);
            foreach (var id in leveransIdList)
            {
                var filloggs = _portalRepository.GetFilloggarForLeveransId(id, DateTime.Now, DateTime.Now);
                foreach (var fillogg in filloggs)
                {
                    var filloggDetalj = (FilloggDetaljDTO.FromFillogg(fillogg));
                    historikLista.Add(filloggDetalj);
                }
            }
            return historikLista;
        }

        public string HamtaKommunKodForAnvandare(string userId)
        {
            var kommunKod = _portalRepository.GetKommunKodForUser(userId);
            return kommunKod;
        }

        public IEnumerable<KommunDetaljDTO> HamtaKommuner()
        {
            throw new NotImplementedException();
        }

        public void SparaTillFillogg(string filNamn, int leveransId)
        {
            _portalRepository.SaveToFilelogg(filNamn,leveransId);
        }

        public int HamtaNyttLeveransId(string rapportorId, string kommunKod)
        {
            var levId = _portalRepository.GetNewLeveransId(rapportorId, kommunKod);
            return levId;
        }
    }
}