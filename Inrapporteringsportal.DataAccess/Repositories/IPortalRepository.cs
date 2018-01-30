using System;
using System.Collections.Generic;
using InrapporteringsPortal.DomainModel;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public interface IPortalRepository
    {
        //Kommun GetByShortName(string shortName);

        //IEnumerable<LevereradFil> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom);

        IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId);

        IEnumerable<int> GetLeveransIdnForOrganisation(int orgId);

        IEnumerable<Leverans> GetLeveranserForOrganisation(int orgId);

        string GetKommunKodForOrganisation(int orgId);

        string GetRegisterKortnamn(int delregId);

        string GetInformationText(string infoTyp);

        int GetNewLeveransId(string userId, string userName, int orgId, int regId, int forvLevId);

        Organisation GetOrgForEmailDomain(string modelEmailDomain);

        Organisation GetOrgForUser(string userId);

        int GetUserOrganisationId(string userId);

        IEnumerable<RegisterInfo> GetAllRegisterInformation();

        void GetPeriodForAktuellLeverans(ICollection<AdmFilkrav> itemAdmFilkrav, RegisterInfo regInfo);

        Aterkoppling GetAterkopplingForLeverans(int levId);
        void SaveToFilelogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        void SaveToLoginLog(string userid, string userName);
    }

}
