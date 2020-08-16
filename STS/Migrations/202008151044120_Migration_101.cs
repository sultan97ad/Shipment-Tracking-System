namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "DeliveryRange", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "DeliveryRange");
        }
    }
}
