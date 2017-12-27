using System;
using System.Collections.Generic;
using InrapporteringsPortal.DomainModel;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public interface IPortalRepository
    {
        Kommun GetByShortName(string shortName);

        //IEnumerable<LevereradFil> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom);

        IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom);

        IEnumerable<int> GetLeveransIdnForOrganisation(int orgId);

        string GetKommunKodForOrganisation(int orgId);

        //string GetKommunKodForUser(string userId);

        void SaveToFilelogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        int GetNewLeveransId(string userId, int orgId, int regId, string period);

        Organisation GetOrgForEmailDomain(string modelEmailDomain);

        int GetUserOrganisation(string userId);
    }

}
