using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using InrapporteringsPortal.DataAccess;
using InrapporteringsPortal.DomainModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InrapporteringsPortal.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            MapEntities(modelBuilder);
        }

        private void MapEntities(DbModelBuilder modelBuilder)
        {
            //Organisation
            modelBuilder.Entity<Organisation>().Property(e => e.Id).HasColumnName("organisationsid");
            modelBuilder.Entity<Organisation>().Property(e => e.Organisationsnr).HasColumnName("organisationsnr");
            modelBuilder.Entity<Organisation>().Property(e => e.Organisationsnamn).HasColumnName("organisationsnamn");
            modelBuilder.Entity<Organisation>().Property(e => e.Hemsida).HasColumnName("hemsida");
            modelBuilder.Entity<Organisation>().Property(e => e.EpostAdress).HasColumnName("epostadress");
            modelBuilder.Entity<Organisation>().Property(e => e.Telefonnr).HasColumnName("telefonnr");
            modelBuilder.Entity<Organisation>().Property(e => e.Adress).HasColumnName("adress");
            modelBuilder.Entity<Organisation>().Property(e => e.Postnr).HasColumnName("postnr");
            modelBuilder.Entity<Organisation>().Property(e => e.Postort).HasColumnName("postort");
            modelBuilder.Entity<Organisation>().Property(e => e.Epostdoman).HasColumnName("epostdoman");
            modelBuilder.Entity<Organisation>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<Organisation>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<Organisation>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<Organisation>().Property(e => e.AndradAv).HasColumnName("andradav");

            //ApplicationUser (kontaktperson)
            modelBuilder.Entity<ApplicationUser>().ToTable("Kontaktperson");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.OrganisationId).HasColumnName("organisationsid");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Namn).HasColumnName("namn");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.AktivFrom).HasColumnName("aktivfrom");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.AktivTom).HasColumnName("aktivtom");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<ApplicationUser>().Property(e => e.AndradAv).HasColumnName("andradav");

            //Kommun
            modelBuilder.Entity<Kommun>().Property(e => e.Id).HasColumnName("organisationsid");
            modelBuilder.Entity<Kommun>().Property(e => e.Kommunkod).HasColumnName("kommunkod");
            modelBuilder.Entity<Kommun>().Property(e => e.Lan).HasColumnName("lan");
            modelBuilder.Entity<Kommun>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<Kommun>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<Kommun>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<Kommun>().Property(e => e.AndradAv).HasColumnName("andradav");
            modelBuilder.Entity<Kommun>().HasRequired(o => o.Organisation).WithOptional();

            //Leverans
            modelBuilder.Entity<Leverans>().Property(e => e.Id).HasColumnName("leveransid");
            modelBuilder.Entity<Leverans>().Property(e => e.OrganisationId).HasColumnName("organisationsid");
            modelBuilder.Entity<Leverans>().Property(e => e.ApplicationUserId).HasColumnName("kontaktpersonid");
            modelBuilder.Entity<Leverans>().Property(e => e.DelregisterId).HasColumnName("delregisterId");
            modelBuilder.Entity<Leverans>().Property(e => e.ForvantadleveransId).HasColumnName("forvantadleveransId");
            modelBuilder.Entity<Leverans>().Property(e => e.Leveranstidpunkt).HasColumnName("leveranstidpunkt");
            modelBuilder.Entity<Leverans>().Property(e => e.Leveransstatus).HasColumnName("leveransstatus");
            modelBuilder.Entity<Leverans>()
                .HasRequired(c => c.AdmForvantadleverans)
                .WithMany(d => d.Leverans)
                .HasForeignKey(c => c.ForvantadleveransId);
            modelBuilder.Entity<Leverans>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<Leverans>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<Leverans>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<Leverans>().Property(e => e.AndradAv).HasColumnName("andradav");

            //Levereradfil
            modelBuilder.Entity<LevereradFil>().Property(e => e.Id).HasColumnName("filid");
            modelBuilder.Entity<LevereradFil>().Property(e => e.LeveransId).HasColumnName("leveransid");
            modelBuilder.Entity<LevereradFil>().Property(e => e.Filnamn).HasColumnName("filnamn");
            modelBuilder.Entity<LevereradFil>().Property(e => e.NyttFilnamn).HasColumnName("nyttfilnamn");
            modelBuilder.Entity<LevereradFil>().Property(e => e.Ordningsnr).HasColumnName("ordningsnr");
            modelBuilder.Entity<LevereradFil>().Property(e => e.Filstatus).HasColumnName("filstatus");
            modelBuilder.Entity<LevereradFil>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<LevereradFil>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<LevereradFil>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<LevereradFil>().Property(e => e.AndradAv).HasColumnName("andradav");

            //AdmRegister
            modelBuilder.Entity<AdmRegister>().ToTable("admRegister");
            modelBuilder.Entity<AdmRegister>().Property(e => e.Id).HasColumnName("registerid");
            modelBuilder.Entity<AdmRegister>().Property(e => e.Registernamn).HasColumnName("registernamn");
            modelBuilder.Entity<AdmRegister>().Property(e => e.Beskrivning).HasColumnName("beskrivning");
            modelBuilder.Entity<AdmRegister>().Property(e => e.Kortnamn).HasColumnName("kortnamn");
            modelBuilder.Entity<AdmRegister>().Property(e => e.Inrapporteringsportal).HasColumnName("inrapporteringsportal");
            modelBuilder.Entity<AdmRegister>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AdmRegister>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AdmRegister>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AdmRegister>().Property(e => e.AndradAv).HasColumnName("andradav");

            //AdmDelregister
            modelBuilder.Entity<AdmDelregister>().ToTable("admDelregister");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Id).HasColumnName("delregisterid");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.RegisterId).HasColumnName("registerid");
            modelBuilder.Entity<AdmDelregister>()
                .HasRequired(c => c.AdmRegister)
                .WithMany(d => d.AdmDelregister)
                .HasForeignKey(c => c.RegisterId);
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Delregisternamn).HasColumnName("delregisternamn");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Beskrivning).HasColumnName("beskrivning");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Kortnamn).HasColumnName("kortnamn");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Slussmapp).HasColumnName("slussmapp");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.Inrapporteringsportal).HasColumnName("inrapporteringsportal");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AdmDelregister>().Property(e => e.AndradAv).HasColumnName("andradav");

            //AdmFilkrav
            modelBuilder.Entity<AdmFilkrav>().ToTable("admFilkrav");
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.Id).HasColumnName("filkravid");
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.DelregisterId).HasColumnName("delregisterid");
            modelBuilder.Entity<AdmFilkrav>()
                .HasRequired(c => c.AdmDelregister)
                .WithMany(d => d.AdmFilkrav)
                .HasForeignKey(c => c.DelregisterId);
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AdmFilkrav>().Property(e => e.AndradAv).HasColumnName("andradav");

            //AdmForvantadleverans
            modelBuilder.Entity<AdmForvantadleverans>().ToTable("admForvantadleverans");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Id).HasColumnName("forvantadleveransid");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.FilkravId).HasColumnName("filkravId");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.DelregisterId).HasColumnName("delregisterid");
            modelBuilder.Entity<AdmForvantadleverans>()
                .HasRequired(c => c.AdmDelregister)
                .WithMany(d => d.AdmForvantadleverans)
                .HasForeignKey(c => c.DelregisterId);
            modelBuilder.Entity<AdmForvantadleverans>()
                .HasRequired(c => c.AdmFilkrav)
                .WithMany(d => d.AdmForvantadleverans)
                .HasForeignKey(c => c.FilkravId);
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Rapporteringsstart).HasColumnName("rapporteringsstart");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Rapporteringsslut).HasColumnName("rapporteringsslut");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Rapporteringsenast).HasColumnName("rapporteringsenast");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Paminnelse1).HasColumnName("paminnelse1");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Paminnelse2).HasColumnName("paminnelse2");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.Paminnelse3).HasColumnName("paminnelse3");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AdmForvantadleverans>().Property(e => e.AndradAv).HasColumnName("andradav");

            //AdmForvantadfil
            modelBuilder.Entity<AdmForvantadfil>().ToTable("admForvantadfil");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.Id).HasColumnName("forvantadfilid");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.FilkravId).HasColumnName("filkravid");
            modelBuilder.Entity<AdmForvantadfil>()
                .HasRequired(c => c.AdmFilkrav)
                .WithMany(d => d.AdmForvantadfil)
                .HasForeignKey(c => c.FilkravId);
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.Filmask).HasColumnName("filmask");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.Regexp).HasColumnName("regexp");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.SkapadDatum).HasColumnName("skapaddatum");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.SkapadAv).HasColumnName("skapadav");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.AndradDatum).HasColumnName("andraddatum");
            modelBuilder.Entity<AdmForvantadfil>().Property(e => e.AndradAv).HasColumnName("andradav");
        }


        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Kommun> Kommun { get; set; }
        public DbSet<Leverans> Leverans { get; set; }
        public DbSet<LevereradFil> LevereradFil { get; set; }
        public DbSet<AdmRegister> AdmRegister { get; set; }
        public DbSet<AdmDelregister> AdmDelregister { get; set; }
        public DbSet<AdmFilkrav> AdmFilkrav { get; set; }
        public DbSet<AdmForvantadfil> AdmForvantadfil { get; set; }
    }
}