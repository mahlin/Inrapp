using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrapporteringsPortal.Models;

namespace InrapporteringsPortal.Web.Models.BusinessLogic
{
    public interface IInrapporteringsPortalService
    {
        //IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId);
        string HamtaKommunKodForOrganisation(int orgId);
        string HamtaKommunKodForAnvandare(string userId);

        void SparaTillFillogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId);

        int HamtaNyttLeveransId(string userId, int orgId, int registerId, string period);

        Organisation GetOrgForEmailDomain(string modelEmail);
        int GetUserOrganisation(string userId);
    }
}
