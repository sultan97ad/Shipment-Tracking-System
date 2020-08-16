namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments");
            DropIndex("dbo.Reports", new[] { "Location_Id" });
            DropIndex("dbo.Reports", new[] { "Shipment_TrackingNumber" });
            DropTable("dbo.Reports");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Reports",
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
            CreateIndex("dbo.Reports", "Shipment_TrackingNumber");
            CreateIndex("dbo.Reports", "Location_Id");
            AddForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments", "TrackingNumber");
            AddForeignKey("dbo.Reports", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
