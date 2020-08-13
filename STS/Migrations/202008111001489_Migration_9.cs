namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "DeliveryLocationLatitude", c => c.Double(nullable: false));
            AddColumn("dbo.Shipments", "DeliveryLocationlongitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "DeliveryLocationlongitude");
            DropColumn("dbo.Shipments", "DeliveryLocationLatitude");
        }
    }
}
