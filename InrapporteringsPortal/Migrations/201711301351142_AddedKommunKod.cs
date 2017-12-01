namespace InrapporteringsPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedKommunKod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "KommunKod", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "KommunKod");
        }
    }
}
