namespace InrapporteringsPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedKommunKod : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "KommunKod", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "KommunKod", c => c.Int(nullable: false));
        }
    }
}
