using System;
using System.Collections.Generic;
using InrapporteringsPortal.DomainModel;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public interface IPortalRepository
    {
        Kommun GetByShortName(string shortName);

        IEnumerable<Fillogg> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom);

        IEnumerable<int> GetLeveransIdnForKommun(int kommunId);
    }

}
