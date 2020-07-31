namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "InService", c => c.Boolean(nullable: false));
            DropColumn("dbo.Locations", "Removed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Removed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Locations", "InService");
        }
    }
}
