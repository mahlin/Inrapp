using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        Leverans GetLatestDeliveryForOrganisationSubDirectoryAndPeriod(int orgId, int subdirId, int forvlevId);
        Leverans GetLatestDeliveryForOrganisationSubDirectoryPeriodAndOrgUnit(int orgId, int subdirId, int forvlevId, int orgUnitId);

        //IEnumerable<int> GetExpectedDeliveriesForSubdirectoryandPeriod(int subdirId, string period);

        string GetKommunKodForOrganisation(int orgId);

        string GetDelregisterKortnamn(int delregId);

        string GetInformationText(string infoTyp);

        string GetInformationTextById(int infoId);

        int GetNewLeveransId(string userId, string userName, int orgId, int regId, int orgenhetsId,int forvLevId, string status);

        Organisation GetOrgForEmailDomain(string modelEmailDomain);

        Organisation GetOrgForUser(string userId);

        IEnumerable<Organisationsenhet> GetOrgUnitsForOrg(int orgId);

        int GetUserOrganisationId(string userId);

        AdmUppgiftsskyldighet GetUppgiftsskyldighetForOrganisationAndRegister(int orgId, int delregid);

        IEnumerable<Organisationsenhet> GetOrganisationUnits(int orgId);

        Organisationsenhet GetOrganisationUnitByCode(string code, int orgId);

        int GetOrganisationsenhetsId(string orgUnitCode, int orgId);
            
        IEnumerable<RegisterInfo> GetAllRegisterInformation();

        IEnumerable<RegisterInfo> GetAllRegisterInformationForOrganisation(int orgId);

        void GetPeriodsForAktuellLeverans(AdmFilkrav filkrav, RegisterFilkrav regfilkrav);

        List<AdmForvantadleverans> GetExpectedDeliveryForSubDirectory(int subDirId);

        int GetExpextedDeliveryIdForSubDirAndPeriod(int subDirId, string period);

        string GetPeriodForAktuellLeverans(int forvLevid);

        

        Aterkoppling GetAterkopplingForLeverans(int levId);

        string GetEnhetskodForLeverans(int orgenhetsid);

        IEnumerable<Roll> GetChosenDelRegistersForUser(string userId);

        IEnumerable<AdmRegister> GetChosenRegistersForUser(string userId);

        string GetUserName(string userId);

        string GetUserContactNumber(string userId);

        string GetUserPhoneNumber(string userId);
        string GetClosedDays();
        string GetClosedFromHour();
        string GetClosedFromMin();
        string GetClosedToHour();
        string GetClosedToMin();
        string GetClosedAnnyway();
        IEnumerable<AdmHelgdag> GetHolidays();

        IEnumerable<AdmSpecialdag> GetSpecialDays();

        IEnumerable<AdmFAQKategori> GetFAQs();
        IEnumerable<string> GetSubDirectoysPeriodsForAYear(int subdirId, int year);

        IEnumerable<AdmDelregister> GetSubdirsForDirectory(int dirId);
        string GetSubDirectoryShortName(int subDirId);

        List<DateTime> GetTaskStartForSubdir(int subdirId);

        DateTime GetReportstartForRegisterAndPeriod(int dirId, string period);

        DateTime GetLatestReportDateForRegisterAndPeriod(int dirId, string period);

        DateTime GetReportstartForRegisterAndPeriodSpecial(int dirId, string period);

        DateTime GetLatestReportDateForRegisterAndPeriodSpecial(int dirId, string period);

        void DisableContact(string userId);

        void EnableContact(string userId);

        void SaveToFilelogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        void SaveToLoginLog(string userid, string userName);

        void SaveChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList);

        void UpdateChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList);

        void UpdateNameForUser(string userId, string userName);

        void UpdateContactNumberForUser(string userId, string number);

        void UpdateActiveFromForUser(string userId);

        void UpdateUserInfo(ApplicationUser user);

        void DeleteDelivery(int deliveryId);

        void DeleteChosenSubDirectoriesForUser(string userId);
    }

}
