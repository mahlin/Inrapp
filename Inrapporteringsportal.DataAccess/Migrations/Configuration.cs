using System.Configuration;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<InrapporteringsPortalDbContext>
    {
        private bool insertTestdata;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InrapporteringsPortal.DataAccess.InrapporteringsPortalDbContext context)
        {
            insertTestdata = bool.TryParse(ConfigurationManager.AppSettings["insert-testdata"], out insertTestdata);
            //if (insertTestdata)
            //{
            //    //Lägg in kontaktpersoner
            //    if (context.AspNetUsers.ToList().Count == 0)
            //    {
            //        var kontaktperson1 = new RegisterViewModel("marie@home.se");
            //        var kontaktperson2 = new Anvandare("aer", "Anna Eriksson");

            //        context.AspNetUsers.AddOrUpdate(kontaktperson1);
            //    }
            //}

            //  This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
                //  to avoid creating duplicate seed data.
            }
        }
}
