namespace InrapporteringsPortal.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIntKommunKod : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "KommunKodOld");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "KommunKodOld", c => c.Int(nullable: false));
        }
    }
}
