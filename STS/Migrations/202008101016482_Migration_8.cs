namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.TrackingRecords", "Shipment_TrackingNumber", "dbo.Shipments");
            DropIndex("dbo.TrackingRecords", new[] { "Location_Id" });
            DropIndex("dbo.TrackingRecords", new[] { "Shipment_TrackingNumber" });
            DropTable("dbo.TrackingRecords");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrackingRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        SignedBy = c.String(),
                        Location_Id = c.Int(),
                        Shipment_TrackingNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.TrackingRecords", "Shipment_TrackingNumber");
            CreateIndex("dbo.TrackingRecords", "Location_Id");
            AddForeignKey("dbo.TrackingRecords", "Shipment_TrackingNumber", "dbo.Shipments", "TrackingNumber");
            AddForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
