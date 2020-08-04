namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "ArrivalDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "ArrivalDate");
        }
    }
}
