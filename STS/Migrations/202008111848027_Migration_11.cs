namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "CollectionMethod", c => c.Byte(nullable: false));
            DropColumn("dbo.Shipments", "Method");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shipments", "Method", c => c.Byte(nullable: false));
            DropColumn("dbo.Shipments", "CollectionMethod");
        }
    }
}
