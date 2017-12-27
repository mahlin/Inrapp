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

        public void SaveToFilelogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId, int sequenceNumber)
        {
            var logFil = new LevereradFil
            {
                LeveransId = leveransId,
                Filnamn = ursprungligtFilNamn,
                NyttFilnamn = nyttFilNamn,
                Ordningsnr = sequenceNumber,
                SkapadDatum = DateTime.Now,
                SkapadAv = "MAH",
                AndradDatum = DateTime.Now,
                AndradAv = "MAH",
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

        public int GetNewLeveransId(string userId, int orgId, int regId, string period)
        {
            var leverans = new Leverans
            {
                OrganisationId = orgId,
                ApplicationUserId = userId,
                DelregisterId = regId,
                Period = "Januari",
                Leveranstidpunkt = DateTime.Now,
                SkapadDatum = DateTime.Now,
                SkapadAv = userId,
                AndradDatum = DateTime.Now,
                AndradAv = userId
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
    }
}