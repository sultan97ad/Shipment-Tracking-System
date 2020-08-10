namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Event = c.Byte(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        SignedBy = c.String(),
                        Location_Id = c.Int(),
                        Shipment_TrackingNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Shipments", t => t.Shipment_TrackingNumber)
                .Index(t => t.Location_Id)
                .Index(t => t.Shipment_TrackingNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments");
            DropForeignKey("dbo.Reports", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Reports", new[] { "Shipment_TrackingNumber" });
            DropIndex("dbo.Reports", new[] { "Location_Id" });
            DropTable("dbo.Reports");
        }
    }
}
