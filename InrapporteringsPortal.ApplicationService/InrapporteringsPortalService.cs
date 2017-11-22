using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Interface;
using Inrapporteringsportal.DataAccess.Repositories;

namespace InrapporteringsPortal.ApplicationService
{
    public class InrapporteringsPortalService : IInrapporteringsPortalService
    {
        private readonly IKommunRepository _kommunRepository;

        public InrapporteringsPortalService(IKommunRepository kommunRepository)
        {
            _kommunRepository = kommunRepository;
        }

        public IEnumerable<KommunDetaljDTO> HamtaKommuner()
        {
            throw new NotImplementedException();
        }
    }
}