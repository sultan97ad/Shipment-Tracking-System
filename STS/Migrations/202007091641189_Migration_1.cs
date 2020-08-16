namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reports", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Reports", new[] { "Location_Id" });
            AlterColumn("dbo.Reports", "Location_Id", c => c.Int());
            CreateIndex("dbo.Reports", "Location_Id");
            AddForeignKey("dbo.Reports", "Location_Id", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Reports", new[] { "Location_Id" });
            AlterColumn("dbo.Reports", "Location_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reports", "Location_Id");
            AddForeignKey("dbo.Reports", "Location_Id", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}
