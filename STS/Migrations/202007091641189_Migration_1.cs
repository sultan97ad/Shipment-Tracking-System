namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations");
            DropIndex("dbo.TrackingRecords", new[] { "Location_Id" });
            AlterColumn("dbo.TrackingRecords", "Location_Id", c => c.Int());
            CreateIndex("dbo.TrackingRecords", "Location_Id");
            AddForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations");
            DropIndex("dbo.TrackingRecords", new[] { "Location_Id" });
            AlterColumn("dbo.TrackingRecords", "Location_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TrackingRecords", "Location_Id");
            AddForeignKey("dbo.TrackingRecords", "Location_Id", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}
