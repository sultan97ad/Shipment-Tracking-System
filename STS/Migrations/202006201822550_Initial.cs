namespace STS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 70),
                        City = c.String(nullable: false, maxLength: 30),
                        CanBeDestination = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        TrackingNumber = c.String(nullable: false, maxLength: 128),
                        SenderName = c.String(nullable: false, maxLength: 90),
                        SenderPhoneNumber = c.String(nullable: false),
                        ReceiverName = c.String(nullable: false, maxLength: 90),
                        ReceiverPhoneNumber = c.String(nullable: false),
                        WeightKG = c.Single(nullable: false),
                        Description = c.String(nullable: false, maxLength: 255),
                        DateAdded = c.DateTime(nullable: false),
                        Status = c.Byte(nullable: false),
                        CurrentLocation_Id = c.Int(),
                        Destination_Id = c.Int(),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.TrackingNumber)
                .ForeignKey("dbo.Locations", t => t.CurrentLocation_Id)
                .ForeignKey("dbo.Locations", t => t.Destination_Id)
                .ForeignKey("dbo.Locations", t => t.Source_Id)
                .Index(t => t.CurrentLocation_Id)
                .Index(t => t.Destination_Id)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        SignedBy = c.String(),
                        Location_Id = c.Int(nullable: false),
                        Shipment_TrackingNumber = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id, cascadeDelete: true)
                .ForeignKey("dbo.Shipments", t => t.Shipment_TrackingNumber, cascadeDelete: true)
                .Index(t => t.Location_Id)
                .Index(t => t.Shipment_TrackingNumber);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EmployeeLocationId = c.Int(nullable: false),
                        EmployeeName = c.String(nullable: false),
                        EmployeeDateAdded = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName] , [EmployeeLocationId] , [EmployeeName] , [EmployeeDateAdded]) VALUES (N'1f1b1af4-33e3-4078-9cec-b282be2e67b0', N'Admin@STS.com', 0, N'AKo/CJ2RDgawt6ukKQ7rpASJkJP8BAasE8jws+Wd4GHfFZUBnOatRQ+mKEla6Amvbg==', N'23d440b7-76d1-4bf2-905e-4a8ab4b9bedc', NULL, 0, 0, NULL, 1, 0, N'Admin@STS.com' , 0 , 'Admin' , N'1997-04-25 00:00:00')");
            Sql("INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'190a0c9e-ccb7-4ee8-ad20-0c9d94bedd56', N'Admin')");
            Sql("INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'd65a5b7d-cc68-4f7e-b957-700afa5b8e38', N'Employee')");
            Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1f1b1af4-33e3-4078-9cec-b282be2e67b0', N'190a0c9e-ccb7-4ee8-ad20-0c9d94bedd56')");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "Shipment_TrackingNumber", "dbo.Shipments");
            DropForeignKey("dbo.Reports", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Shipments", "Source_Id", "dbo.Locations");
            DropForeignKey("dbo.Shipments", "Destination_Id", "dbo.Locations");
            DropForeignKey("dbo.Shipments", "CurrentLocation_Id", "dbo.Locations");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Reports", new[] { "Shipment_TrackingNumber" });
            DropIndex("dbo.Reports", new[] { "Location_Id" });
            DropIndex("dbo.Shipments", new[] { "Source_Id" });
            DropIndex("dbo.Shipments", new[] { "Destination_Id" });
            DropIndex("dbo.Shipments", new[] { "CurrentLocation_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Reports");
            DropTable("dbo.Shipments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Locations");
        }
    }
}
