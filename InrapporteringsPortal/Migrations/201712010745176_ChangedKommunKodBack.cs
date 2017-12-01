namespace InrapporteringsPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedKommunKodBack : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "KommunKodOld", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "KommunKod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "KommunKod", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "KommunKodOld");
        }
    }
}
