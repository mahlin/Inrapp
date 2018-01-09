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

        public Kommun GetByShortName(string shortName)
        {
            throw new NotImplementedException();
        }

        ////TODO - 
        //public IEnumerable<LevereradFil> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom)
        //{
        //    //var tmp2 = (from l in DbContext.Leverans
        //    //    where l.Id == 1
        //    //    select l).SingleOrDefault();

        //    //var tmp1 = (from f in DbContext.Fillogg
        //    //    where f.LeveransId == 1
        //    //            select f).SingleOrDefault(); 

        //    //var tmp = (from f in DbContext.Fillogg
        //    //    where f.LeveransId == leveransId
        //    //    orderby f.Id 
        //    //    select f).ToList();

        //    var filloggar = AllaFilloggar().Where(a => a.LeveransId == leveransId).OrderByDescending(x => x.LeveransId); ;

        //    return filloggar;
        //}

        //TODO - 
        public IEnumerable<LevereradFil> GetFilerForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom)
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
            return DbContext.Organisation.Include(x => x.Kommuner).Include(x => x.Users);
        }


        public IEnumerable<int> GetLeveransIdnForOrganisation(int orgId)
        {
            var levIdnForOrg = AllaLeveranser().Where(a => a.OrganisationId == orgId).Select(a => a.Id).ToList();

            return levIdnForOrg;

        }

        public string GetKommunKodForOrganisation(int orgId)
        {
            //var tfnNr = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.PhoneNumber).FirstOrDefault();
            var kommunKod = DbContext.Kommun.Where(x => x.Id == orgId).Select(x => x.Kommunkod).FirstOrDefault();
            return kommunKod;
        }

        //public string GetKommunKodForUser(string userId)
        //{
        //    //var tfnNr = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.PhoneNumber).FirstOrDefault();
        //    //TODO - ska AspNetUsers finnas i DomainModel? ApplicationUser/IdentityUser ligger i DataAccess
        //    //Jmfr video - ändrar ApplicationUser till User + DbContext/InModelCreating
        //    //var orgId = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.OrganisationsId).FirstOrDefault();
        //    var orgId = 1;
        //    var kommunKod = DbContext.Kommun.Where(a => a.OrganisationsId == orgId).Select(a => a.KommunKod).FirstOrDefault();
        //    return kommunKod;
        //}

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

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //ErrorManager.WriteToErrorLog("FileUploaderController", "Upload", e.ToString());
                throw new Exception(e.Message);
            }
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

        public int GetUserOrganisation(string userId)
        {
            var orgId = DbContext.Users.Where(u => u.Id == userId).Select(o => o.OrganisationId).SingleOrDefault();
            return orgId;
        }

        public IEnumerable<RegisterInfo> GetAllRegisterInformation()
        {
            var registerInfoList = new List<RegisterInfo>();

            var register = DbContext.AdmRegister.ToList();
            var tmp = DbContext.AdmRegister.Include(x => x.AdmDelregister).ToList();
            var tmp2 = DbContext.AdmDelregister.Include(f => f.AdmFilkrav).ToList();
            var tmp3 = DbContext.AdmDelregister.Include(f => f.AdmFilkrav.Select(q => q.AdmForvantadfil)).ToList();

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
                //if (item.AdmFilkrav.Count > 0)
                //{
                //    var forvantadFil = item.AdmFilkrav.Select(x => x.AdmForvantadfil).Single();
                //    regInfo.FilMask = forvantadFil.Select(x => x.Filmask).FirstOrDefault();
                //    regInfo.RegExp = forvantadFil.Select(x => x.Regexp).FirstOrDefault();
                //}

                var filmaskList = new List<string>();
                var regExpList = new List<string>();


                //Antal filer, filmask samt regexp
                if (item.AdmFilkrav.Count > 0) //TODO - kan komma fler? Antar endast en så länge
                {
                    var forvantadFil = item.AdmFilkrav.Select(x => x.AdmForvantadfil).ToList();

                    regInfo.AntalFiler = forvantadFil.Count;
                    regInfo.InfoText = regInfo.InfoText + "<br> Antal filer: " + regInfo.AntalFiler.ToString();
                    foreach (var forvFil in forvantadFil)
                    {
                        filmaskList.Add(forvFil.Select(x => x.Filmask).FirstOrDefault());
                        regExpList.Add(forvFil.Select(x => x.Regexp).FirstOrDefault());
                        regInfo.InfoText = regInfo.InfoText + "<br> Filformat: " + forvFil.Select(x => x.Filmask).FirstOrDefault();
                    }
                    //get period och forvantadleveransId
                    GetPeriodForAktuellLeverans(item.AdmFilkrav, regInfo);
                }

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
                //För varje filkrav - hämta förväntad leverans med rätt period utifrån dagens datum
                var forvantadLeverans = filkrav.AdmForvantadleverans.FirstOrDefault();
                if (forvantadLeverans != null)
                {
                    startDate = forvantadLeverans.Rapporteringsstart;
                    endDate = forvantadLeverans.Rapporteringsslut;
                    if (dagensDatum >= startDate && dagensDatum <= endDate)
                    {
                        regInfo.Period = forvantadLeverans.Period;
                        regInfo.ForvantadLevransId = forvantadLeverans.Id;
                    }
                }
                
            }
        }

        public void SaveToLoginLog(string userid)
        {
            var inloggning = new Inloggning
            {
                ApplicationUserId = userid,
                Inloggningstidpunkt = DateTime.Now
            };

            DbContext.Inloggning.Add(inloggning);

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
    }
}