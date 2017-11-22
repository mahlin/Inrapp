using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InrapporteringsPortal.DomainModel;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public class KommunRepository : IKommunRepository
    {
        private InrapporteringsPortalDbContext DbContext { get; }
        private readonly Repository<Kommun, InrapporteringsPortalDbContext> _internalGenericRepository;

        public KommunRepository(InrapporteringsPortalDbContext dbContext)
        {
            DbContext = dbContext;
            _internalGenericRepository = new Repository<Kommun, InrapporteringsPortalDbContext>(DbContext);
        }

        public Kommun GetByShortName(string shortName)
        {
            throw new NotImplementedException();
        }
    }
}