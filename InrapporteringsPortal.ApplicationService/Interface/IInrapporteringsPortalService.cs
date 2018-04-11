using System;
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

        IEnumerable<FilloggDetaljDTO> FiltreraHistorikForAnvandare(string userId, IEnumerable<FilloggDetaljDTO> historikForOrganisation);

        string HamtaKommunKodForOrganisation(int orgId);
        string HamtaKommunKodForAnvandare(string userId);

        int HamtaNyttLeveransId(string userId, string userName, int orgId, int registerId, int orgenhetsId, int forvLevId);

        string HamtaInformationsText(string infoTyp);

        Organisation HamtaOrgForEmailDomain(string modelEmail);

        Organisation HamtaOrgForAnvandare(string userId);

        string HamtaAnvandaresNamn(string userId);

        string HamtaAnvandaresMobilnummer(string userId);

        int HamtaUserOrganisationId(string userId);

        IEnumerable<RegisterInfo> HamtaAllRegisterInformation();

        IEnumerable<RegisterInfo> HamtaValdaRegistersForAnvandare(string userId, int orgId);

        IEnumerable<RegisterInfo> HamtaRegistersMedAnvandaresVal(string userId);

        AdmUppgiftsskyldighet HamtaUppgiftsskyldighetForOrganisationOchRegister(int orgId, int delregid);

        int HamtaForvantadleveransIdForRegisterOchPeriod(int delregId, string period);

        IEnumerable<AdmFAQKategori> HamtaFAQs();

        List<string> HamtaGiltigaPerioderForDelregister(int delregId);

        void SparaTillDatabasFillogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        void SparaValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> registerList);

        void UppdateraValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> registerList);

        void UppdateraNamnForAnvandare(string userId, string userName);

        void UppdateraAktivFromForAnvandare(string userId);
        void UppdateraAnvandarInfo(ApplicationUser user);

        void SaveToLoginLog(string userid, string userName);

        string MaskPhoneNumber(string phoneNumber);

        bool IsOpen();
    }
}
