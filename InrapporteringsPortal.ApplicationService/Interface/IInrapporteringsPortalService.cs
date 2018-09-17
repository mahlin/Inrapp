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
        IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisationRegisterPeriod(int orgId, int regId,string periodForReg);

        IEnumerable<FilloggDetaljDTO> FiltreraHistorikForAnvandare(string userId, IEnumerable<FilloggDetaljDTO> historikForOrganisation);

        string HamtaKommunKodForOrganisation(int orgId);
        string HamtaKommunKodForAnvandare(string userId);

        int HamtaNyttLeveransId(string userId, string userName, int orgId, int registerId, int orgenhetsId, int forvLevId, string status);

        string HamtaInformationsText(string infoTyp);
        string HamtaInformationsTextMedId(int infoId);

        string HamtaHelgEllerSpecialdagsInfo();

        string HamtaHelgdagsInfo();

        string HamtaSpecialdagsInfo();

        Organisation HamtaOrgForEmailDomain(string modelEmail);

        Organisation HamtaOrgForAnvandare(string userId);

        string HamtaAnvandaresNamn(string userId);

        string HamtaAnvandaresKontaktnummer(string userId);

        string HamtaAnvandaresMobilnummer(string userId);

        int HamtaUserOrganisationId(string userId);

        Organisationsenhet HamtaOrganisationsenhetMedEnhetskod(string kod, int orgId);

        IEnumerable<RegisterInfo> HamtaAllRegisterInformation();

        IEnumerable<RegisterInfo> HamtaValdaRegistersForAnvandare(string userId, int orgId);

        IEnumerable<RegisterInfo> HamtaRegistersMedAnvandaresVal(string userId, int orgId);

        IEnumerable<AdmRegister> HamtaRegisterForAnvandare(string userId, int orgId);

        AdmUppgiftsskyldighet HamtaUppgiftsskyldighetForOrganisationOchRegister(int orgId, int delregid);

        int HamtaForvantadleveransIdForRegisterOchPeriod(int delregId, string period);

        IEnumerable<AdmForvantadfil> HamtaForvantadFil(int filkravId);

        IEnumerable<AdmFAQKategori> HamtaFAQs();

        IEnumerable<string> HamtaDelregistersPerioderForAr(int delregId, int ar);
        List<string> HamtaGiltigaPerioderForDelregister(int delregId);

        string HamtaSammanlagdStatusForPeriod(IEnumerable<FilloggDetaljDTO> historikLista);

        List<int> HamtaValbaraAr(int delregId);

        DateTime HamtaRapporteringsstartForRegisterOchPeriod(int regId, string period);

        DateTime HamtaSenasteRapporteringForRegisterOchPeriod(int regId, string period);

        DateTime HamtaRapporteringsstartForRegisterOchPeriodSpecial(int regId, string period);

        DateTime HamtaSenasteRapporteringForRegisterOchPeriodSpecial(int regId, string period);

        void InaktiveraKontaktperson(string userId);

        void AktiveraKontaktperson(string userId);

        void SparaTillDatabasFillogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber);

        void SparaValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> registerList);

        void UppdateraValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> registerList);

        void UppdateraNamnForAnvandare(string userId, string userName);

        void UppdateraKontaktnummerForAnvandare(string userId, string tfnnr);

        void UppdateraAktivFromForAnvandare(string userId);
        void UppdateraAnvandarInfo(ApplicationUser user);

        void SaveToLoginLog(string userid, string userName);

        string MaskPhoneNumber(string phoneNumber);

        bool IsOpen();
        
        bool IsHelgdag();

        bool IsSpecialdag();
        string ClosedComingWeek();
        string HelgdagComingWeek();
        string SpecialdagComingWeek();
    }
}
