namespace InrapporteringsPortal.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Int(nullable: false),
                        TwoFactorEnabled = c.Int(nullable: false),
                        LockoutEndDateUtc = c.DateTime(nullable: false),
                        LockoutEnabled = c.Int(nullable: false),
                        UserName = c.String(),
                        KommunKod = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Filloggs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeveransId = c.Int(nullable: false),
                        Filnamn = c.String(),
                        Datum = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Leverans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReporterId = c.String(),
                        CountyId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Leverans");
            DropTable("dbo.Filloggs");
            DropTable("dbo.AspNetUsers");
        }
    }
}
