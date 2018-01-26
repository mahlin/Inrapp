﻿using System;
using System.Collections.Generic;
using InrapporteringsPortal.ApplicationService.DTOModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.ApplicationService.Interface
{
    public interface IInrapporteringsPortalService
    {
        IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId);
        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId);

        string HamtaKommunKodForOrganisation(int orgId);
        string HamtaKommunKodForAnvandare(string userId);

        int HamtaNyttLeveransId(string userId, string userName, int orgId, int registerId, int forvLevId);

        string HamtaInformationsText(string infoTyp);

        Organisation HamtaOrgForEmailDomain(string modelEmail);

        Organisation HamtaOrgForAnvandare(string userId);

        int HamtaUserOrganisationId(string userId);

        void SparaTillDatabasFillogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        IEnumerable<RegisterInfo> HamtaAllRegisterInformation();

        void SaveToLoginLog(string userid);
    }
}
