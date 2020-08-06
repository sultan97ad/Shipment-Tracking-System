namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Locations", "longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "longitude");
            DropColumn("dbo.Locations", "Latitude");
        }
    }
}
