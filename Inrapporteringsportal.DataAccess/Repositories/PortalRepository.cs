using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;


namespace Inrapporteringsportal.DataAccess.Repositories
{
    public class PortalRepository : IPortalRepository
    {
        private ApplicationDbContext DbContext { get; }

        public PortalRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            //DbContext.Configuration.ProxyCreationEnabled = false;
        }


        public IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId)
        {
            //var tmp2 = (from l in DbContext.Leverans
            //    where l.Id == 1
            //    select l).SingleOrDefault();

            //var tmp1 = (from f in DbContext.Fillogg
            //    where f.LeveransId == 1
            //            select f).SingleOrDefault(); 

            //var tmp = (from f in DbContext.Fillogg
            //    where f.LeveransId == leveransId
            //    orderby f.Id 
            //    select f).ToList();

            var filInfo = AllaFiler().Where(a => a.LeveransId == leveransId).OrderByDescending(x => x.LeveransId); ;

            return filInfo;
        }

        private IEnumerable<LevereradFil> AllaFiler()
        {
            //return DbContext.Fillogg.Include(x => x.Leverans).Include(x => x.Reporter);
            return DbContext.LevereradFil;
        }

        private IEnumerable<Leverans> AllaLeveranser()
        {
            return DbContext.Leverans;
        }

        private IEnumerable<Organisation> AllaOrganisationer()
        {
            return DbContext.Organisation.Include(x => x.Users);
        }


        public IEnumerable<int> GetLeveransIdnForOrganisation(int orgId)
        {
            var levIdnForOrg = AllaLeveranser().Where(a => a.OrganisationId == orgId).Select(a => a.Id).ToList();

            return levIdnForOrg;

        }

        public IEnumerable<Leverans> GetLeveranserForOrganisation(int orgId)
        {
            var levIdnForOrg = AllaLeveranser().Where(a => a.OrganisationId == orgId).ToList();

            return levIdnForOrg;

        }

        public Leverans GetLatestDeliveryForOrganisationSubDirectoryAndPeriod(int orgId, int subdirId, int forvlevId)
        {
            var latestsDeliveryForOrgAndSubdirectory = AllaLeveranser()
                .Where(a => a.OrganisationId == orgId && a.DelregisterId == subdirId &&
                            a.ForvantadleveransId == forvlevId).OrderByDescending(x => x.Id).FirstOrDefault();
            return latestsDeliveryForOrgAndSubdirectory;
        }

        public string GetKommunKodForOrganisation(int orgId)
        {
            //var tfnNr = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.PhoneNumber).FirstOrDefault();
            var kommunKod = DbContext.Organisation.Where(x => x.Id == orgId).Select(x => x.Kommunkod).FirstOrDefault();
            return kommunKod;
        }


        public void SaveToFilelogg(string userName, string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber)
        {
            var logFil = new LevereradFil
            {
                LeveransId = leveransId,
                Filnamn = ursprungligtFilNamn,
                NyttFilnamn = nyttFilNamn,
                Ordningsnr = sequenceNumber,
                SkapadDatum = DateTime.Now,
                SkapadAv = userName,
                AndradDatum = DateTime.Now,
                AndradAv = userName,
                Filstatus = "Levererad"
            };

            DbContext.LevereradFil.Add(logFil);

            DbContext.SaveChanges();
        }

        public int GetNewLeveransId(string userId, string userName, int orgId, int regId, int orgenhetsId, int forvLevId, string status)
        {
            var dbStatus = "Levererad";
            if (!String.IsNullOrEmpty(status))
            {
                dbStatus = status;
            }
            var leverans = new Leverans
            {
                ForvantadleveransId = forvLevId,
                OrganisationId = orgId,
                ApplicationUserId = userId,
                DelregisterId = regId,
                Leveranstidpunkt = DateTime.Now,
                Leveransstatus = dbStatus,
                SkapadDatum = DateTime.Now,
                SkapadAv = userName,
                AndradDatum = DateTime.Now,
                AndradAv = userName
            };

            if (orgenhetsId != 0)
            {
                leverans.OrganisationsenhetsId = orgenhetsId;
            }
            

            DbContext.Leverans.Add(leverans);

            DbContext.SaveChanges();
            return leverans.Id;
        }

        public Organisation GetOrgForEmailDomain(string modelEmailDomain)
        {
            var organisation = DbContext.Organisation.Where(a => a.Epostdoman == modelEmailDomain).Select(o => o).FirstOrDefault();
            return organisation;
        }

        public int GetUserOrganisationId(string userId)
        {
            var orgId = DbContext.Users.Where(u => u.Id == userId).Select(o => o.OrganisationId).SingleOrDefault();
            return orgId;
        }

        public IEnumerable<RegisterInfo> GetAllRegisterInformationForOrganisation(int orgId)
        {
            var registerInfoList = new List<RegisterInfo>();

            var uppgSkyldighetDelRegIds = DbContext.AdmUppgiftsskyldighet.Where(x => x.OrganisationId == orgId).Select(x => x.DelregisterId).ToList();

            var delregister = DbContext.AdmDelregister
                .Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadfil))
                .Where(x => x.Inrapporteringsportal && uppgSkyldighetDelRegIds.Contains(x.Id))
                .ToList();


            foreach (var item in delregister)
            {
                var regInfoObj = CreateRegisterInfoObj(item);
                registerInfoList.Add(regInfoObj);
            }

            return registerInfoList;
        }


        public IEnumerable<RegisterInfo> GetAllRegisterInformation()
        {
            var registerInfoList = new List<RegisterInfo>();

            var delregister = DbContext.AdmDelregister
                .Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadfil))
                .Where(x => x.Inrapporteringsportal)
                .ToList();


            foreach (var item in delregister)
            {
                var regInfoObj = CreateRegisterInfoObj(item);
                registerInfoList.Add(regInfoObj);
            }

            return registerInfoList;
        }

        private RegisterInfo CreateRegisterInfoObj(AdmDelregister delReg)
        {
            var regInfo = new RegisterInfo
            {
                Id = delReg.Id,
                Namn = delReg.Delregisternamn,
                Kortnamn = delReg.Kortnamn,
                InfoText = delReg.AdmRegister.Beskrivning + "<br>" + delReg.Beskrivning,
                Slussmapp = delReg.Slussmapp
            };
            

            var filkravList = new List<RegisterFilkrav>();
            var i = 1;

            foreach (var filkrav in delReg.AdmFilkrav)
            {
                var regFilkrav = new RegisterFilkrav();
                var filmaskList = new List<string>();
                var regExpList = new List<string>();
                if (filkrav.Namn != null)
                {
                    regFilkrav.Namn = filkrav.Namn;
                }
                else
                {
                    regFilkrav.Namn = "";
                }   

                //Sök forväntad fil för varje filkrav istället för alla forvantade filer för registret!!
                //var forvantadFil = delReg.AdmFilkrav.Select(x => x.AdmForvantadfil);
                var forvantadeFiler = filkrav.AdmForvantadfil.ToList();
                regFilkrav.AntalFiler = forvantadeFiler.Count();

                foreach (var forvFil in forvantadeFiler)
                {
                    filmaskList.Add(forvFil.Filmask);
                    regExpList.Add(forvFil.Regexp);
                    regFilkrav.InfoText = regFilkrav.InfoText + "<br> Filformat: " + forvFil.Filmask;
                    regFilkrav.Obligatorisk = forvFil.Obligatorisk;
                }
                
                //get period och forvantadleveransId
                GetPeriodsForAktuellLeverans(filkrav, regFilkrav);
                regFilkrav.InfoText = regFilkrav.InfoText + "<br> Antal filer: " + regFilkrav.AntalFiler;
                regFilkrav.Id = i;
                regFilkrav.FilMasker = filmaskList;
                regFilkrav.RegExper = regExpList;


                //Om inga aktuella perioder finns för filkravet ska det inte läggas med i RegInfo
                if (regFilkrav.Perioder != null)
                {
                    if (regFilkrav.Perioder.ToList().Count != 0)
                    {
                        filkravList.Add(regFilkrav);
                        i++;
                    }
                }
            }

            regInfo.Filkrav = filkravList;
            return regInfo;
        }

        public void GetPeriodsForAktuellLeverans(AdmFilkrav filkrav, RegisterFilkrav regFilkrav)
        {
            string period = String.Empty;
            DateTime startDate;
            DateTime endDate;

            DateTime dagensDatum = DateTime.Now.Date;
            var perioder = new List<string>();

            //hämta varje förväntad leverans och sätt rätt period utifrån dagens datum
            foreach (var item in filkrav.AdmForvantadleverans)
            {
                if (item != null)
                {
                    startDate = item.Rapporteringsstart;
                    endDate = item.Rapporteringsslut;
                    if (dagensDatum >= startDate && dagensDatum <= endDate)
                    {
                        //regInfo.Period = item.Period;
                        perioder.Add(item.Period);
                        //regInfo.ForvantadLevransId = item.Id;
                    }
                }
                regFilkrav.Perioder = perioder;
            }
        }

        public void SaveToLoginLog(string userid, string userName)
        {
            var inloggning = new Inloggning
            {
                ApplicationUserId = userid,
                SkapadDatum = DateTime.Now,
                SkapadAv = userName
            };

            DbContext.Inloggning.Add(inloggning);

            DbContext.SaveChanges();

        }

        public string GetDelregisterKortnamn(int delregId)
        {
            //var register = DbContext.AdmRegister.ToList();
            //var tmp = DbContext.AdmRegister.Include(x => x.AdmDelregister).ToList();
            //var tmp2 = DbContext.AdmDelregister.Include(f => f.AdmFilkrav).ToList();
            var namn = DbContext.AdmDelregister.Where(x => x.Id == delregId).Select(q => q.Kortnamn).FirstOrDefault();
            return namn;

        }

        public string GetInformationText(string infoTyp)
        {
            var text = DbContext.AdmInformation.Where(x => x.Informationstyp == infoTyp).Select(q => q.Text).SingleOrDefault();
            return text;
        }

        public string GetInformationTextById(int infoId)
        {
            var text = DbContext.AdmInformation.Where(x => x.Id == infoId).Select(q => q.Text).SingleOrDefault();
            return text;
        }

        public Organisation GetOrgForUser(string userId)
        {
            var orgId = GetUserOrganisationId(userId);

            var org = AllaOrganisationer().Where(o => o.Id == orgId).Select(o => o).FirstOrDefault();

            return org;
        }

        public string GetUserName(string userId)
        {
            var userName = DbContext.Users.Where(u => u.Id == userId).Select(u => u.Namn).SingleOrDefault();
            return userName;
        }

        public string GetUserContactNumber(string userId)
        {
            var userContactNumber = DbContext.Users.Where(u => u.Id == userId).Select(u => u.Kontaktnummer).SingleOrDefault();
            return userContactNumber;
        }

        public string GetUserPhoneNumber(string userId)
        {
            var phoneNumber = DbContext.Users.Where(u => u.Id == userId).Select(u => u.PhoneNumber).SingleOrDefault();
            return phoneNumber;
        }

        public Aterkoppling GetAterkopplingForLeverans(int levId)
        {
            var aterkoppling = DbContext.Aterkoppling.Where(x => x.LeveransId == levId).FirstOrDefault();
            return aterkoppling;
        }

        public string GetEnhetskodForLeverans(int orgenhetsid)
        {
            var enhetskod = DbContext.Organisationsenhet.Where(x => x.Id == orgenhetsid).Select(x => x.Enhetskod).SingleOrDefault();
            return enhetskod;
        }

        public IEnumerable<Roll> GetChosenDelRegistersForUser(string userId)
        {
            var rollList = new List<Roll>();

            rollList = DbContext.Roll.Where(x => x.ApplicationUserId == userId).ToList();

            return rollList;
        }

        public IEnumerable<AdmRegister> GetChosenRegistersForUser(string userId)
        {
            var rollList = new List<Roll>();
            var registerList = new List<AdmRegister>();
            var regIdList = new List<int>(); 

            rollList = DbContext.Roll.Where(x => x.ApplicationUserId == userId).ToList();

            foreach (var roll in rollList)
            {
                var tmp = DbContext.AdmDelregister.Where(x => x.Id == roll.DelregisterId).FirstOrDefault();
                var delregister = DbContext.AdmDelregister.SingleOrDefault(x => x.Id == roll.DelregisterId);
                var registerId = DbContext.AdmRegister.Where(x => x.Id == delregister.RegisterId).Select(x => x.Id)
                    .SingleOrDefault();
                regIdList.Add(registerId);
            }

            //rensa bort dubbletter och hämta delregister för varje register
            var regIdListDistinct = regIdList.Distinct();
            foreach (var registerId in regIdListDistinct)
            {
                var register = DbContext.AdmRegister.Where(x => x.Id == registerId).Include(x => x.AdmDelregister).SingleOrDefault();
                registerList.Add(register);
            }
            return registerList;
        }

        public string GetPeriodForAktuellLeverans(int forvLevid)
        {
            var period = DbContext.AdmForvantadleverans.Where(x => x.Id == forvLevid).Select(x => x.Period).SingleOrDefault();
            return period;
        }

        public List<AdmForvantadleverans> GetExpectedDeliveryForSubDirectory(int subDirId)
        {
            var periods = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDirId).ToList();
            return periods;
        }

        public int GetExpextedDeliveryIdForSubDirAndPeriod(int subDirId, string period)
        {
            var forvLevId = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDirId && x.Period == period)
                .Select(x => x.Id).SingleOrDefault();
            return forvLevId;
        }

        public IEnumerable<string> GetSubDirectoysPeriodsForAYear(int subdirId, int year)
        {
            var dateFrom = new DateTime(year,01,01);
            var dateTom = new DateTime(year, 12, 31).Date;
            var periods = DbContext.AdmForvantadleverans
                .Where(x => x.DelregisterId == subdirId && x.Uppgiftsstart >= dateFrom && x.Uppgiftsslut <= dateTom)
                .Select(x => x.Period).ToList();

            return periods;
        }

        public void SaveChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList)
        {
            foreach (var register in registerList)
            {
                if (register.Selected)
                {
                    var roll = new Roll
                    {
                        DelregisterId = register.Id,
                        ApplicationUserId = userId,
                        SkapadDatum = DateTime.Now,
                        SkapadAv = userName,
                        AndradDatum = DateTime.Now,
                        AndradAv = userName
                    };

                    DbContext.Roll.Add(roll);
                }
            }
            DbContext.SaveChanges();
        }

        public void UpdateChosenRegistersForUser(string userId, string userName, List<RegisterInfo> registerList)
        {
            //delete prevoious choices
            DbContext.Roll.RemoveRange(DbContext.Roll.Where(x => x.ApplicationUserId == userId));

            //Insert new choices
            foreach (var register in registerList)
            {
                if (register.Selected)
                {
                    var roll = new Roll
                    {
                        DelregisterId = register.Id,
                        ApplicationUserId = userId,
                        SkapadDatum = DateTime.Now,
                        SkapadAv = userName,
                        AndradDatum = DateTime.Now,
                        AndradAv = userName
                    };

                    DbContext.Roll.Add(roll);
                }
            }
            DbContext.SaveChanges();
        }

        public void UpdateNameForUser(string userId, string userName)
        {
            var user = DbContext.Users.Where(u => u.Id == userId).Select(u => u).SingleOrDefault();
            user.Namn = userName;
            DbContext.SaveChanges();
        }


        public void UpdateContactNumberForUser(string userId, string number)
        {
            var user = DbContext.Users.Where(u => u.Id == userId).Select(u => u).SingleOrDefault();
            user.Kontaktnummer = number;
            DbContext.SaveChanges();
        }

        public void UpdateActiveFromForUser(string userId)
        {
            var user = DbContext.Users.Where(u => u.Id == userId).Select(u => u).SingleOrDefault();
            user.AktivFrom = DateTime.Now;
            DbContext.SaveChanges();
        }

        public void UpdateUserInfo(ApplicationUser user)
        {
            var userDb = DbContext.Users.SingleOrDefault(x => x.Id == user.Id);
            userDb.AndradAv = user.AndradAv;
            userDb.AndradDatum = user.AndradDatum;
            DbContext.SaveChanges();
        }

        public AdmUppgiftsskyldighet GetUppgiftsskyldighetForOrganisationAndRegister(int orgId, int delregid)
        {
            var uppgiftsskyldighet = DbContext.AdmUppgiftsskyldighet.SingleOrDefault(x => x.OrganisationId == orgId && x.DelregisterId == delregid);

            return uppgiftsskyldighet;
        }

        public IEnumerable<Organisationsenhet> GetOrganisationUnits(int orgId)
        {
            var orgUnits = DbContext.Organisationsenhet.Where(x => x.OrganisationsId == orgId).ToList();
            return orgUnits;
        }

        public int GetOrganisationsenhetsId(string orgUnitCode, int orgId)
        {
            var orgenhetsId = DbContext.Organisationsenhet.Where(x => x.Enhetskod == orgUnitCode && x.OrganisationsId == orgId).Select(x => x.Id).FirstOrDefault();
            return orgenhetsId;
        }

        public string GetClosedFromHour()
        {
            var closedFromHour = "0";
            closedFromHour = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedFromHour").Select(x => x.Varde).SingleOrDefault();
            return closedFromHour;
        }

        public string GetClosedFromMin()
        {
            var closedFromMin = "0";
            closedFromMin = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedFromMin").Select(x => x.Varde).SingleOrDefault();
            return closedFromMin;
        }

        public string GetClosedToHour()
        {
            var closedToHour = "0";
            closedToHour = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedToHour").Select(x => x.Varde).SingleOrDefault();
            return closedToHour;
        }

        public string GetClosedToMin()
        {
            var closedToMin = "0";
            closedToMin = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedToMin").Select(x => x.Varde).SingleOrDefault();
            return closedToMin;
        }

        public string GetClosedAnnyway()
        {
            var closedAnyway = "false";
            closedAnyway = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedAnyway").Select(x => x.Varde).SingleOrDefault();
            return closedAnyway;
        }

        public IEnumerable<AdmHelgdag> GetHolidays()
        {
            var holidays = DbContext.AdmHelgdag.ToList();
            return holidays;
        }

        public IEnumerable<AdmSpecialdag> GetSpecialDays()
        {
            var specialDays = DbContext.AdmSpecialdag.ToList();
            return specialDays;
        }

        public string GetClosedDays()
        {
            var closedDays = String.Empty;
            closedDays = DbContext.AdmKonfiguration.Where(x => x.Typ == "ClosedDays").Select(x => x.Varde).SingleOrDefault();
            return closedDays;
        }
        
        public IEnumerable<AdmFAQKategori> GetFAQs()
        {
            //var faqs = DbContext.AdmFAQKategori.Include(x => x.AdmFAQ).OrderBy(x => x.Sortering).ToList();
            var faqs = DbContext.AdmFAQKategori.OrderBy(x => x.Sortering).ToList();

            //Hämta FAQs per kategori separat (pga orderby)
            foreach (var faqCat in faqs)
            {
                faqCat.AdmFAQ = DbContext.AdmFAQ.Where(x => x.FAQkategoriId == faqCat.Id).OrderBy(x => x.Sortering).ToList();
            }

            return faqs;
           
        }

        public IEnumerable<AdmDelregister> GetSubdirsForDirectory(int dirId)
        {
            var subdirectories = DbContext.AdmDelregister.Where(x => x.RegisterId == dirId).ToList();
            return subdirectories;
        }

        public List<DateTime> GetTaskStartForSubdir(int subdirId)
        {
            var taskstartList = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subdirId)
                .Select(x => x.Uppgiftsstart).ToList();

            return taskstartList;
        }

        public DateTime GetReportstartForRegisterAndPeriod(int dirId, string period)
        {
            var firstSubDirForReg = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId);
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == firstSubDirForReg.Id && x.Period == period)
                .Select(x => x.Rapporteringsstart).FirstOrDefault();

            return reportstart;
        }

        public DateTime GetLatestReportDateForRegisterAndPeriod(int dirId, string period)
        {
            var firstSubDirForReg = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId);
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == firstSubDirForReg.Id && x.Period == period)
                .Select(x => x.Rapporteringsenast).FirstOrDefault();

            return reportstart;
        }

        //TODO - special för EKB-År, Lös på annat sätt. Db?
        public DateTime GetReportstartForRegisterAndPeriodSpecial(int dirId, string period)
        {
            var subDir = DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId && x.Kortnamn == "EKB-År");
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id && x.Period == period)
                .Select(x => x.Rapporteringsstart).FirstOrDefault();

            return reportstart;
        }

        public DateTime GetLatestReportDateForRegisterAndPeriodSpecial(int dirId, string period)
        {
            var subDir= DbContext.AdmDelregister.FirstOrDefault(x => x.RegisterId == dirId && x.Kortnamn == "EKB-År");
            var reportstart = DbContext.AdmForvantadleverans.Where(x => x.DelregisterId == subDir.Id && x.Period == period)
                .Select(x => x.Rapporteringsenast).FirstOrDefault();

            return reportstart;
        }

        public void DeleteDelivery(int deliveryId)
        {
            var deliveryToDelete = DbContext.Leverans.SingleOrDefault(x => x.Id == deliveryId);
            DbContext.Leverans.Remove(deliveryToDelete);
            DbContext.SaveChanges();
        }

        public void DisableContact(string userId)
        {
            var contactDb = DbContext.Users.SingleOrDefault(x => x.Id == userId);
            contactDb.AktivTom = DateTime.Now;
            DbContext.SaveChanges();
        }

        public void EnableContact(string userId)
        {
            var contactDb = DbContext.Users.SingleOrDefault(x => x.Id == userId);
            contactDb.AktivTom = null;
            DbContext.SaveChanges();
        }

        public void DeleteChosenSubDirectoriesForUser(string userId)
        {
            var rollList = DbContext.Roll.Where(x => x.ApplicationUserId == userId).ToList();
            DbContext.Roll.RemoveRange(rollList);
            DbContext.SaveChanges();
        }


    }
}