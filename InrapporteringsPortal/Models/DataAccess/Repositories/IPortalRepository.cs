﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrapporteringsPortal.Models;

namespace InrapporteringsPortal.Web.Models.DataAccess.Repositories
{
    public interface IPortalRepository
    {
        Kommun GetByShortName(string shortName);

        IEnumerable<LevereradFil> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom);

        IEnumerable<int> GetLeveransIdnForOrganisation(int orgId);

        string GetKommunKodForOrganisation(int orgId);

        //string GetKommunKodForUser(string userId);

        void SaveToFilelogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId);

        int GetNewLeveransId(string userId, int orgId, int regId, string period);

        Organisation GetOrgForEmailDomain(string modelEmailDomain);

        int GetUserOrganisation(string userId);
    }
}
