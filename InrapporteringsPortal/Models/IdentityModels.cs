using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Dynamic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InrapporteringsPortal.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public string KommunKod { get; set; }
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DbContext", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
    //        //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

    //        //modelBuilder.Entity<ApplicationUser>().ToTable("Kontaktperson");
    //        //modelBuilder.Entity<IdentityRole>().ToTable("Roles");
    //        //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
    //        //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
    //        //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
    //    }
    //}
}