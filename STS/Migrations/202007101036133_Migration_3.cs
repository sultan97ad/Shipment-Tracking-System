namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Removed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Removed");
        }
    }
}
