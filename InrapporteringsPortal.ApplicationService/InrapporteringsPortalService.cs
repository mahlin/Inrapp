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

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(int kommunId)
        {
            var historikLista = new List<FilloggDetaljDTO>();
            //TODO - tidsintervall
            var leveransIdList = _portalRepository.GetLeveransIdnForKommun(kommunId);
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

        public IEnumerable<KommunDetaljDTO> HamtaKommuner()
        {
            throw new NotImplementedException();
        }
    }
}