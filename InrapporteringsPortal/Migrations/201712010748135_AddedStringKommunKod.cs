namespace InrapporteringsPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStringKommunKod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "KommunKod", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "KommunKod");
        }
    }
}
