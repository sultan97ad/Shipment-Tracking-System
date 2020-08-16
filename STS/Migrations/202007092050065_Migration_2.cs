namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments");
            DropIndex("dbo.Reports", new[] { "Shipment_TrackingNumber" });
            AlterColumn("dbo.Locations", "LocationName", c => c.String(maxLength: 70));
            AlterColumn("dbo.Locations", "City", c => c.String(maxLength: 30));
            AlterColumn("dbo.Shipments", "SenderName", c => c.String(maxLength: 90));
            AlterColumn("dbo.Shipments", "SenderPhoneNumber", c => c.String());
            AlterColumn("dbo.Shipments", "ReceiverName", c => c.String(maxLength: 90));
            AlterColumn("dbo.Shipments", "ReceiverPhoneNumber", c => c.String());
            AlterColumn("dbo.Shipments", "Description", c => c.String(maxLength: 255));
            AlterColumn("dbo.Reports", "Shipment_TrackingNumber", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reports", "Shipment_TrackingNumber");
            AddForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments", "TrackingNumber");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments");
            DropIndex("dbo.Reports", new[] { "Shipment_TrackingNumber" });
            AlterColumn("dbo.Reports", "Shipment_TrackingNumber", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Shipments", "Description", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Shipments", "ReceiverPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Shipments", "ReceiverName", c => c.String(nullable: false, maxLength: 90));
            AlterColumn("dbo.Shipments", "SenderPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Shipments", "SenderName", c => c.String(nullable: false, maxLength: 90));
            AlterColumn("dbo.Locations", "City", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Locations", "LocationName", c => c.String(nullable: false, maxLength: 70));
            CreateIndex("dbo.Reports", "Shipment_TrackingNumber");
            AddForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments", "TrackingNumber", cascadeDelete: true);
        }
    }
}
