namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "Method", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "Method");
        }
    }
}
