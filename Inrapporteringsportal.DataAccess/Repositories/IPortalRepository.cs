﻿using System;
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

        int GetNewLeveransId(string userId, string userName, int orgId, int regId, int orgenhetsId,int forvLevId);

        Organisation GetOrgForEmailDomain(string modelEmailDomain);

        Organisation GetOrgForUser(string userId);

        int GetUserOrganisationId(string userId);

        AdmUppgiftsskyldighet GetUppgiftsskyldighetForOrganisationAndRegister(int orgId, int delregid);

        IEnumerable<Organisationsenhet> GetOrganisationUnits(int orgId);

        int GetOrganisationsenhetsId(string orgUnitCode, int orgId);
            
        IEnumerable<RegisterInfo> GetAllRegisterInformation();

        void GetPeriodForAktuellLeverans(ICollection<AdmFilkrav> itemAdmFilkrav, RegisterInfo regInfo);

        int GetForvantadleveransIdForRegisterAndPeriod(int delregId, string period);

        Aterkoppling GetAterkopplingForLeverans(int levId);

        string GetEnhetskodForLeverans(int orgenhetsid);

        IEnumerable<Roll> GetChosenRegistersForUser(string userId);

        string GetUserName(string userId);

        string GetUserPhoneNumber(string userId);
        string GetClosedDays();
        string GetClosedFromHour();
        string GetClosedFromMin();
        string GetClosedToHour();
        string GetClosedToMin();
        string GetClosedAnnyway();

        IEnumerable<AdmFAQKategori> GetFAQs();

        void SaveToFilelogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        void SaveToLoginLog(string userid, string userName);

        void SaveChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList);

        void UpdateChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList);

        void UpdateNameForUser(string userId, string userName);

        void UpdateActiveFromForUser(string userId);
    }

}
