using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Diagnostics;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.DataAccess
{
    public class InrapporteringsPortalDbContext : DbContext
    {
        public InrapporteringsPortalDbContext() : base("name=DbContext")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public InrapporteringsPortalDbContext(string connString) : base(connString)
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<Fillogg> Fillogg { get; set; }
        public DbSet<Leverans> Leverans { get; set; }
    }
}