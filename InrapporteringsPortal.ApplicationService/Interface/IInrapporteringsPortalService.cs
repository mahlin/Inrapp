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
        string HamtaKommunKodForOrganisation(int orgId);
        string HamtaKommunKodForAnvandare(string userId);

        void SparaTillFillogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId);

        int HamtaNyttLeveransId(string rapportorId, string kommunKod);

        Organisation GetOrgForEmailDomain(string modelEmail);
    }
}
