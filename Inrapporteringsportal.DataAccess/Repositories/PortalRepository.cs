using System;
using System.Collections.Generic;
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
        private InrapporteringsPortalDbContext DbContext { get; }
        private readonly Repository<Kommun, InrapporteringsPortalDbContext> _internalGenericRepository;

        public PortalRepository(InrapporteringsPortalDbContext dbContext)
        {
            DbContext = dbContext;
            _internalGenericRepository = new Repository<Kommun, InrapporteringsPortalDbContext>(DbContext);
        }

        public Kommun GetByShortName(string shortName)
        {
            throw new NotImplementedException();
        }

        //TODO - 
        public IEnumerable<Fillogg> GetFilloggarForLeveransId(int leveransId, DateTime datumFrom, DateTime datumTom)
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

            var filloggar = AllaFilloggar().Where(a => a.LeveransId == leveransId).OrderByDescending(x => x.LeveransId); ;

            return filloggar;
        }

        private IEnumerable<Fillogg> AllaFilloggar()
        {
            //return DbContext.Fillogg.Include(x => x.Leverans).Include(x => x.Reporter);
            return DbContext.Fillogg;
        }

        private IEnumerable<Leverans> AllaLeveranser()
        {
            return DbContext.Leverans;
        }

        private IEnumerable<AspNetUsers> AllaUsers()
        {
            return DbContext.AspNetUsers;
        }

        public IEnumerable<int> GetLeveransIdnForKommun(string kommunId)
        {
            var levIdnForKommun = AllaLeveranser().Where(a => a.CountyId == kommunId).Select(a => a.Id).ToList();

            return levIdnForKommun;

        }

        public string GetKommunKodForUser(string userId)
        {
            //var tfnNr = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.PhoneNumber).FirstOrDefault();
            var kommunKod = DbContext.AspNetUsers.Where(a => a.Id == userId).Select(a => a.KommunKod).FirstOrDefault();
            return kommunKod;
        }

        public void SaveToFilelogg(string ursprungligtFilNamn, string nyttFilNamn, int leveransId)
        {
            var logOrg = new Fillogg
            {
                LeveransId = leveransId,
                Filnamn = ursprungligtFilNamn,
                Datum = DateTime.Now,
                Status = 1
            };

            DbContext.Fillogg.Add(logOrg);

            var logNew = new Fillogg
            {
                LeveransId = leveransId,
                Filnamn = nyttFilNamn,
                Datum = DateTime.Now,
                Status = 1
            };

            DbContext.Fillogg.Add(logNew);
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

        public int GetNewLeveransId(string rapportorId, string kommunKod)
        {
            var leverans = new Leverans
            {
                ReporterId = rapportorId,
                CountyId = kommunKod
            };

            DbContext.Leverans.Add(leverans);

            DbContext.SaveChanges();
            return leverans.Id;
        }

    }
}