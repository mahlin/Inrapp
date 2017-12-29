using System.Configuration;

namespace InrapporteringsPortal.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InrapporteringsPortal.DataAccess.ApplicationDbContext>
    {
        private bool insertTestdata;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InrapporteringsPortal.DataAccess.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            insertTestdata = bool.TryParse(ConfigurationManager.AppSettings["insert-testdata"], out insertTestdata);
            if (insertTestdata)
            {
                if (context.Organisation.ToList().Count == 0)
                {
                    
                }
            }
        }
    }
}
