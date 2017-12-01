using System;
using System.Collections.Generic;
using InrapporteringsPortal.ApplicationService.DTOModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InrapporteringsPortal.ApplicationService.Interface
{
    public interface IInrapporteringsPortalService
    {
        IEnumerable<KommunDetaljDTO> HamtaKommuner();
        IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId);
        string HamtaKommunKodForAnvandare(string userId);

        void SparaTillFillogg(string filNamn, int leveransId);

        int HamtaNyttLeveransId(string rapportorId, string kommunKod);
    }
}
