using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.DynamicData;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;


namespace Inrapporteringsportal.DataAccess.Repositories
{
    public class PortalRepository : IPortalRepository
    {
        private ApplicationDbContext DbContext { get; }

        public PortalRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }


        //TODO - 
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

        public int GetNewLeveransId(string userId, string userName, int orgId, int regId, int forvLevId)
        {
            //TODO - sätt SkapdAv resp AndradAv = UserName när kolumnen är lika stor, dvs nvarchar(256)
            var leverans = new Leverans
            {
                ForvantadleveransId = forvLevId,
                OrganisationId = orgId,
                ApplicationUserId = userId,
                DelregisterId = regId,
                Leveranstidpunkt = DateTime.Now,
                Leveransstatus = "Levererad",
                SkapadDatum = DateTime.Now,
                SkapadAv = userName,
                AndradDatum = DateTime.Now,
                AndradAv = userName
            };

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

        public IEnumerable<RegisterInfo> GetAllRegisterInformation()
        {
            var registerInfoList = new List<RegisterInfo>();

            var delregister = DbContext.AdmDelregister
                .Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadfil))
                .Where(x => x.Inrapporteringsportal)
                .ToList();

            foreach (var item in delregister)
            {

                var regInfo = new RegisterInfo
                {
                    Id = item.Id,
                    Namn = item.Delregisternamn,
                    Kortnamn = item.Kortnamn,
                    InfoText = item.AdmRegister.Beskrivning + "<br>" + item.Beskrivning,
                    Slussmapp = item.Slussmapp,
                };

                var filmaskList = new List<string>();
                var regExpList = new List<string>();

                //Antal filer, filmask samt regexp
                if (item.AdmFilkrav.Count > 0) //TODO - kan komma fler? Antar endast en så länge
                {
                    var forvantadFil= item.AdmFilkrav.Select(x => x.AdmForvantadfil);

                    foreach (var forvFil in forvantadFil)
                    {
                        regInfo.AntalFiler = forvFil.Count();
                        foreach (var fil in forvFil)
                        {
                            filmaskList.Add(fil.Filmask);
                            regExpList.Add(fil.Regexp);
                            regInfo.InfoText = regInfo.InfoText + "<br> Filformat: " + fil.Filmask;
                        }
                    }
                    //get period och forvantadleveransId
                    GetPeriodForAktuellLeverans(item.AdmFilkrav, regInfo);
                }

                regInfo.InfoText = regInfo.InfoText + "<br> Antal filer: " + regInfo.AntalFiler.ToString();
                regInfo.FilMasker = filmaskList;
                regInfo.RegExper = regExpList;

                registerInfoList.Add(regInfo);
            }

            return registerInfoList;
        }

        public void GetPeriodForAktuellLeverans(ICollection<AdmFilkrav> itemAdmFilkrav, RegisterInfo regInfo)
        {
            string period = String.Empty;
            DateTime startDate;
            DateTime endDate;

            DateTime dagensDatum = DateTime.Now;

            foreach (var filkrav in itemAdmFilkrav) //Todo - kan vara fler? Antar endast en så länge
            {
                //För varje filkrav - hämta varje förväntad leverans och sätt rätt period utifrån dagens datum
                foreach (var item in filkrav.AdmForvantadleverans)
                {
                    if (item != null)
                    {
                        startDate = item.Rapporteringsstart;
                        endDate = item.Rapporteringsslut;
                        if (dagensDatum >= startDate && dagensDatum <= endDate)
                        {
                            regInfo.Period = item.Period;
                            regInfo.ForvantadLevransId = item.Id;
                        }
                    }
                }
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

        public string GetRegisterKortnamn(int delregId)
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

        public Organisation GetOrgForUser(string userId)
        {
            var orgId = GetUserOrganisationId(userId);

            var org = AllaOrganisationer().Where(o => o.Id == orgId).Select(o => o).FirstOrDefault();

            return org;

        }

        public Aterkoppling GetAterkopplingForLeverans(int levId)
        {
            var aterkoppling = DbContext.Aterkoppling.Where(x => x.LeveransId == levId).FirstOrDefault();
            return aterkoppling;
        }

        public IEnumerable<Roll> GetChosenRegistersForUser(int userId)
        {
            var rollList = new List<Roll>();

            rollList = DbContext.Roll.Where(x => x.ApplicationUserId == userId).ToList();

            return rollList;
        }

        public void SaveChosenRegistersForUser(int userId, string userName, List<int> regIdList)
        {
            foreach (var regId in regIdList)
            {
                var roll = new Roll
                {
                    DelregisterId = regId,
                    ApplicationUserId = userId,
                    SkapadDatum = DateTime.Now,
                    SkapadAv = userName
                };

                DbContext.Roll.Add(roll);
            }
            DbContext.SaveChanges();
        }

        public void UpdateChosenRegistersForUser(int userId, string userName, List<int> regIdList)
        {
            //delete prevoious choices
            DbContext.Roll.RemoveRange(DbContext.Roll.Where(x => x.ApplicationUserId == userId));

            //Insert new choices
            foreach (var regId in regIdList)
            {
                var roll = new Roll
                {
                    DelregisterId = regId,
                    ApplicationUserId = userId,
                    SkapadDatum = DateTime.Now,
                    SkapadAv = userName
                };

                DbContext.Roll.Add(roll);
            }
            DbContext.SaveChanges();
        }
    }
}