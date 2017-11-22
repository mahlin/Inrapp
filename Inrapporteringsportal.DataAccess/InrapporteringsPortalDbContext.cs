using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Diagnostics;

namespace Inrapporteringsportal.DataAccess.Repositories
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
    }
}