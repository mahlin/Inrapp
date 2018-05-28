using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Web;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Interface;
using Inrapporteringsportal.DataAccess.Repositories;
using InrapporteringsPortal.DomainModel;
using InrapporteringsPortal.ApplicationService.Helpers;

namespace InrapporteringsPortal.ApplicationService
{
    public class InrapporteringsPortalService : IInrapporteringsPortalService
    {
        private readonly IPortalRepository _portalRepository;

        public InrapporteringsPortalService(IPortalRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForKommun(string kommunId)
        {
            throw new NotImplementedException();
            //var historikLista = new List<FilloggDetaljDTO>();
            ////TODO - tidsintervall
            //var leveransIdList = _portalRepository.GetLeveransIdnForKommun(kommunId).OrderByDescending(x => x);
            //foreach (var id in leveransIdList)
            //{
            //    var filloggs = _portalRepository.GetFilloggarForLeveransId(id, DateTime.Now, DateTime.Now);
            //    foreach (var fillogg in filloggs)
            //    {
            //        var filloggDetalj = (FilloggDetaljDTO.FromFillogg(fillogg));
            //        historikLista.Add(filloggDetalj);
            //    }
            //}
            //return historikLista;
        }

        public IEnumerable<FilloggDetaljDTO> HamtaHistorikForOrganisation(int orgId)
        {
            var historikLista = new List<FilloggDetaljDTO>();
            //TODO - tidsintervall?
            //var leveransIdList = _portalRepository.GetLeveransIdnForOrganisation(orgId).OrderByDescending(x => x);
            var leveransList = _portalRepository.GetLeveranserForOrganisation(orgId);
            foreach (var leverans in leveransList)
            {
                var filloggDetalj = new FilloggDetaljDTO();
                //Kolla om återkopplingsfil finns för aktuell leverans
                var aterkoppling = _portalRepository.GetAterkopplingForLeverans(leverans.Id);

                //Kolla om enhetskod finns för aktuell leverans (stadsdelsleverans)
                var enhetskod = String.Empty;
                if (leverans.OrganisationsenhetsId != null)
                {
                    var orgenhetid = Convert.ToInt32(leverans.OrganisationsenhetsId);
                    enhetskod = _portalRepository.GetEnhetskodForLeverans(orgenhetid);
                }

                //Hämta period för aktuell leverans
                var period = _portalRepository.GetPeriodForAktuellLeverans(leverans.ForvantadleveransId);

                var filer = _portalRepository.GetFilerForLeveransId(leverans.Id).ToList();
                var registerKortnamn = _portalRepository.GetRegisterKortnamn(leverans.DelregisterId);

                if (!filer.Any())
                {
                    filloggDetalj = new FilloggDetaljDTO();
                    filloggDetalj.Id = 0;
                    filloggDetalj.LeveransId = leverans.Id;
                    filloggDetalj.Filnamn = " - ";
                    filloggDetalj.Filstatus = " - ";
                    filloggDetalj.Kontaktperson = leverans.ApplicationUserId;
                    filloggDetalj.Leveransstatus = leverans.Leveransstatus;
                    filloggDetalj.Leveranstidpunkt = leverans.Leveranstidpunkt;
                    filloggDetalj.RegisterKortnamn = registerKortnamn;
                    filloggDetalj.Resultatfil = " - ";
                    filloggDetalj.Enhetskod = enhetskod;
                    filloggDetalj.Period = period;
                    if (aterkoppling != null)
                    {
                        filloggDetalj.Leveransstatus = aterkoppling.Leveransstatus;
                        filloggDetalj.Resultatfil = aterkoppling.Resultatfil;
                    }
                    historikLista.Add(filloggDetalj);
                }
                else
                {
                    foreach (var fil in filer)
                    {
                        filloggDetalj = (FilloggDetaljDTO.FromFillogg(fil));
                        filloggDetalj.Kontaktperson = leverans.ApplicationUserId;
                        filloggDetalj.Leveransstatus = leverans.Leveransstatus;
                        filloggDetalj.Leveranstidpunkt = leverans.Leveranstidpunkt;
                        filloggDetalj.RegisterKortnamn = registerKortnamn;
                        filloggDetalj.Resultatfil = "Ej kontrollerad";
                        filloggDetalj.Enhetskod = enhetskod;
                        filloggDetalj.Period = period;
                        if (aterkoppling != null)
                        {
                            filloggDetalj.Leveransstatus = aterkoppling.Leveransstatus;
                            filloggDetalj.Resultatfil = aterkoppling.Resultatfil;
                        }
                        historikLista.Add(filloggDetalj);
                    }
                }
            }
            var sorteradHistorikLista = historikLista.OrderByDescending(x => x.Leveranstidpunkt).ToList();

            return sorteradHistorikLista;
        }

        public string HamtaKommunKodForAnvandare(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisationId(userId);
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }
        
        public void SparaTillDatabasFillogg(string userName, string ursprungligFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber)
        {
            _portalRepository.SaveToFilelogg(userName, ursprungligFilNamn, nyttFilNamn, leveransId, sequenceNumber);
        }

        public int HamtaNyttLeveransId(string userId, string userName, int orgId, int registerId, int orgenhetsId, int forvLevId, string status)
        {
            var levId = _portalRepository.GetNewLeveransId(userId, userName, orgId, registerId, orgenhetsId, forvLevId, status);
            return levId;
        }

        public Organisation HamtaOrgForEmailDomain(string modelEmail)
        {
            MailAddress address = new MailAddress(modelEmail);
            string host = address.Host;
            //Check if Host contains multiple parts, get the last one
            string domain = GetLastPartOfHostAdress(host);

            var organisation= _portalRepository.GetOrgForEmailDomain(domain);
            return organisation;
        }

        public string HamtaKommunKodForOrganisation(int orgId)
        {
            var kommunKod = _portalRepository.GetKommunKodForOrganisation(orgId);
            return kommunKod;
        }

        public int HamtaUserOrganisationId(string userId)
        {
            var orgId = _portalRepository.GetUserOrganisationId(userId);
            return orgId;
        }

        public IEnumerable<RegisterInfo> HamtaAllRegisterInformation()
        {
            var registerList = _portalRepository.GetAllRegisterInformation();
            return registerList;
        }

        public void SaveToLoginLog(string userid, string userName)
        {
            _portalRepository.SaveToLoginLog(userid, userName);
        }

        public string HamtaInformationsText(string infoTyp)
        {
            var infoText = _portalRepository.GetInformationText(infoTyp);
            return infoText;
        }

        public string HamtaInformationsTextMedId(int infoId)
        {
            var infoText = _portalRepository.GetInformationTextById(infoId);
            return infoText;
        }

        

        public Organisation HamtaOrgForAnvandare(string userId)
        {
            var org = _portalRepository.GetOrgForUser(userId);
            return org;
        }

        public IEnumerable<RegisterInfo> HamtaValdaRegistersForAnvandare(string userId, int orgId)
        {
            var registerList = _portalRepository.GetChosenDelRegistersForUser(userId);
            //var allaRegisterList = _portalRepository.GetAllRegisterInformation();
            var allaRegisterList = _portalRepository.GetAllRegisterInformationForOrganisation(orgId);
            var userRegisterList = new List<RegisterInfo>();

            foreach (var register in allaRegisterList)
            {
                foreach (var userRegister in registerList)
                {
                    if (register.Id == userRegister.DelregisterId)
                    {
                        register.SelectedFilkrav = "0";
                        userRegisterList.Add(register);
                    }
                }
            }

            //Check if users organisation reports per unit. If thats the case, get list of units
            foreach (var item in userRegisterList)
            {
                var uppgiftsskyldighet = HamtaUppgiftsskyldighetForOrganisationOchRegister(orgId, item.Id);
                if (uppgiftsskyldighet.RapporterarPerEnhet)
                {
                    item.RapporterarPerEnhet = true;
                    var orgUnits = _portalRepository.GetOrganisationUnits(orgId);
                    item.Organisationsenheter = new List<KeyValuePair<string, string>>();
                    foreach (var orgUnit in orgUnits)
                    {
                        KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(orgUnit.Enhetskod, orgUnit.Enhetsnamn);
                        item.Organisationsenheter.Add(keyValuePair); 
                    }
                    
                }
            }

            return userRegisterList;
        }

        public void SparaValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> regIdList)
        {
            _portalRepository.SaveChosenRegistersForUser(userId,userName,regIdList);
        }

        public void UppdateraValdaRegistersForAnvandare(string userId, string userName, List<RegisterInfo> registerList)
        {
            _portalRepository.UpdateChosenRegistersForUser(userId,userName, registerList);
        }

        public IEnumerable<RegisterInfo> HamtaRegistersMedAnvandaresVal(string userId, int orgId)
        {
            var registerList = _portalRepository.GetChosenDelRegistersForUser(userId);
            //var allaRegisterList = _portalRepository.GetAllRegisterInformation();
            var allaRegisterList = _portalRepository.GetAllRegisterInformationForOrganisation(orgId);

            foreach (var register in allaRegisterList)
            {
                foreach (var userRegister in registerList)
                {
                    if (register.Id == userRegister.DelregisterId)
                    {
                        register.Selected = true;
                    }
                }
            }

            return allaRegisterList;
        }


        public IEnumerable<AdmRegister> HamtaRegisterForAnvandare(string userId, int orgId)
        {
            var registerList = _portalRepository.GetChosenRegistersForUser(userId);
            //Rensa bort dubbletter avseende kortnamn
            //IEnumerable<string> allaKortnamn = registerList.Select(x => x.Kortnamn).Distinct();

            return registerList;
        }
        public void UppdateraNamnForAnvandare(string userId, string userName)
        {
            _portalRepository.UpdateNameForUser(userId,userName);
        }

        public void UppdateraKontaktnummerForAnvandare(string userId, string tfnnr)
        {
            _portalRepository.UpdateContactNumberForUser(userId,tfnnr);
        }

        public void UppdateraAktivFromForAnvandare(string userId)
        {
            _portalRepository.UpdateActiveFromForUser(userId);
        }

        public void UppdateraAnvandarInfo(ApplicationUser user)
        {
            _portalRepository.UpdateUserInfo(user);
        }

        public string HamtaAnvandaresNamn(string userId)
        {
            var userName = _portalRepository.GetUserName(userId);
            return userName;
        }

        public string HamtaAnvandaresKontaktnummer(string userId)
        {
            var contactNumber = _portalRepository.GetUserContactNumber(userId);
            return contactNumber;
        }

        public string HamtaAnvandaresMobilnummer(string userId)
        {
            var phoneNumber = _portalRepository.GetUserPhoneNumber(userId);
            return phoneNumber;
        }

        public List<string> HamtaGiltigaPerioderForDelregister(int delregId)
        {
            var allaForvLevForDelreg = _portalRepository.GetExpectedDeliveryForSubDirectory(delregId);
            string period = String.Empty;
            DateTime startDate;
            DateTime endDate;

            DateTime dagensDatum = DateTime.Now.Date;
            var perioder = new List<string>();

            foreach (var forvLev in allaForvLevForDelreg)
            {
                startDate = forvLev.Rapporteringsstart;
                endDate = forvLev.Rapporteringsslut;
                if (dagensDatum >= startDate && dagensDatum <= endDate)
                {
                    perioder.Add(forvLev.Period);
                }
            }
            return perioder;
        }

        public string MaskPhoneNumber(string phoneNumber)
        {
            var maskedPhoneNumber = String.Empty;

            //Replace initial numbers with *
            var lengthToMask = phoneNumber.Length - 4;
            for (int i = 0; i < lengthToMask; i++)
            {
                maskedPhoneNumber += "*";
            }
            //Add four last number to masked string
            maskedPhoneNumber += phoneNumber.Substring(lengthToMask);

            return maskedPhoneNumber;
        }

        public AdmUppgiftsskyldighet HamtaUppgiftsskyldighetForOrganisationOchRegister(int orgId, int delregid)
        {
            var uppgiftsskyldighet = _portalRepository.GetUppgiftsskyldighetForOrganisationAndRegister(orgId, delregid);

            return uppgiftsskyldighet;
        }

        public string HamtaHelgEllerSpecialdagsInfo()
        {
            var text = String.Empty;
            if (IsHelgdag())
            {
                text = HamtaHelgdagsInfo();
            }
            else if (IsSpecialdag())
            {
                text = HamtaSpecialdagsInfo();
            }

            return text;
        }

        public string HamtaHelgdagsInfo()
        {
            var date = DateTime.Now.Date;
            var text = String.Empty;
            var helgdagar = _portalRepository.GetHolidays();

            foreach (var helgdag in helgdagar)
            {
                if (helgdag.Helgdatum == date)
                {
                    text = _portalRepository.GetInformationTextById(helgdag.InformationsId);
                }
            }

            return text;
        }

        public string HamtaSpecialdagsInfo()
        {
            var date = DateTime.Now.Date;
            var text = String.Empty;
            var specialdagar = _portalRepository.GetSpecialDays();

            foreach (var dag in specialdagar)
            {
                if (dag.Specialdagdatum == date)
                {
                    text = _portalRepository.GetInformationTextById(dag.InformationsId);
                }
            }

            return text;
        }

        public bool IsHelgdag()
        {
            var date = DateTime.Now.Date;
            var helgdagar = _portalRepository.GetHolidays();

            foreach (var helgdag in helgdagar)
            {
                if (helgdag.Helgdatum == date)
                    return true;
            }

            return false;
        }

        public bool IsSpecialdag()
        {
            var now = DateTime.Now;
            var date = DateTime.Now.Date;

            var timeNow = now.TimeOfDay;
            var specialdagar = _portalRepository.GetSpecialDays();

            foreach (var dag in specialdagar)
            {
                if (dag.Specialdagdatum == date)
                    //Kolla klockslag
                    if (dag.Oppna >= timeNow || dag.Stang <= timeNow)
                    {
                        return true;
                    }
            }
            return false;
        }

        public bool IsOpen()
        {
            if (IsHelgdag())
            {
                return false;
            }
            else if (IsSpecialdag())
            {
                return false;
            }

            var now = DateTime.Now;
            var today = now.DayOfWeek.ToString();
            var hourNow = now.Hour;
            var minuteNow = now.Minute;

            //Get Openinghours from database
            var closedDays = _portalRepository.GetClosedDays().Split(new char[] { ',' }); 
            var closedFromHour = Int32.Parse(_portalRepository.GetClosedFromHour());
            var closedFromMin = Int32.Parse(_portalRepository.GetClosedFromMin());
            var closedToHour = Int32.Parse(_portalRepository.GetClosedToHour());
            var closedToMin = Int32.Parse(_portalRepository.GetClosedToMin());
            var closedAnyway = Convert.ToBoolean(_portalRepository.GetClosedAnnyway());

            //Test
            //hourNow = 8;
            //minuteNow = 2;

            if (closedAnyway)
            {
                return false;
            }

            //Check day
            foreach (var day in closedDays)
            {
                if (day == today)
                {
                    return false;
                }
            }

            //After closing
            if ((closedFromHour <= hourNow))
            {
                //Check minute
                if (minuteNow < closedFromMin)
                {
                    return true;
                }
                return false;
            }

            //Before opening?
            if ((closedToHour > hourNow))
            {
                return false;
            }

            if (closedToHour == hourNow)
            {
                //Check minute
                if (minuteNow > closedToMin)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        private string GetLastPartOfHostAdress(string hostAdress)
        {
            List<int> indexes = hostAdress.AllIndexesOf(".");

            if (indexes.Count == 1)
                return hostAdress;

            var indexToCutFrom = indexes[indexes.Count - 2];
            return hostAdress.Substring(indexToCutFrom + 1);

        }

        public IEnumerable<FilloggDetaljDTO> FiltreraHistorikForAnvandare(string userId, IEnumerable<FilloggDetaljDTO> historikForOrganisation)
        {
            var userOrg = HamtaOrgForAnvandare(userId);
            var registerInfoList = HamtaValdaRegistersForAnvandare(userId, userOrg.Id);

            var historikForAnvandareList = new List<FilloggDetaljDTO>();

            foreach (var rad in historikForOrganisation)
            {
                foreach (var valtRegister in registerInfoList)
                {
                    if (rad.RegisterKortnamn == valtRegister.Kortnamn)
                    {
                        historikForAnvandareList.Add(rad);
                    }
                }

            }
            return historikForAnvandareList;
        }

        public int HamtaForvantadleveransIdForRegisterOchPeriod(int delregId, string period)
        {
            var forvLevId = _portalRepository.GetForvantadleveransIdForRegisterAndPeriod(delregId, period);
            return forvLevId;
        }

        public IEnumerable<AdmFAQKategori> HamtaFAQs()
        {
            var faqs = _portalRepository.GetFAQs();
            return faqs;
        }


    }
}