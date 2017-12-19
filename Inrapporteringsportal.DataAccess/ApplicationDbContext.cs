using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using InrapporteringsPortal.DataAccess.IdentityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InrapporteringsPortal.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<ApplicationUser>().ToTable("Kontaktperson");
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
        }
    }
}