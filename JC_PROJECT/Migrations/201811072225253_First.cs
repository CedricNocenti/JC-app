namespace JC_PROJECT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AUTH_DEV.AspNetRoles",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "AUTH_DEV.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RoleId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("AUTH_DEV.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("AUTH_DEV.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "AUTH_DEV.AspNetUsers",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Email = c.String(nullable: false, maxLength: 128),
                        EmailConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        PasswordHash = c.String(maxLength: 256),
                        SecurityStamp = c.String(maxLength: 256),
                        PhoneNumber = c.String(maxLength: 256),
                        PhoneNumberConfirmed = c.Decimal(nullable: false, precision: 1, scale: 0),
                        TwoFactorEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Decimal(nullable: false, precision: 1, scale: 0),
                        AccessFailedCount = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UserName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "AUTH_DEV.AspNetUserClaims",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        ClaimType = c.String(maxLength: 256),
                        ClaimValue = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("AUTH_DEV.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "AUTH_DEV.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey })
                .ForeignKey("AUTH_DEV.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("AUTH_DEV.AspNetUserRoles", "UserId", "AUTH_DEV.AspNetUsers");
            DropForeignKey("AUTH_DEV.AspNetUserLogins", "UserId", "AUTH_DEV.AspNetUsers");
            DropForeignKey("AUTH_DEV.AspNetUserClaims", "UserId", "AUTH_DEV.AspNetUsers");
            DropForeignKey("AUTH_DEV.AspNetUserRoles", "RoleId", "AUTH_DEV.AspNetRoles");
            DropIndex("AUTH_DEV.AspNetUserLogins", new[] { "UserId" });
            DropIndex("AUTH_DEV.AspNetUserClaims", new[] { "UserId" });
            DropIndex("AUTH_DEV.AspNetUsers", "UserNameIndex");
            DropIndex("AUTH_DEV.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("AUTH_DEV.AspNetUserRoles", new[] { "UserId" });
            DropIndex("AUTH_DEV.AspNetRoles", "RoleNameIndex");
            DropTable("AUTH_DEV.AspNetUserLogins");
            DropTable("AUTH_DEV.AspNetUserClaims");
            DropTable("AUTH_DEV.AspNetUsers");
            DropTable("AUTH_DEV.AspNetUserRoles");
            DropTable("AUTH_DEV.AspNetRoles");
        }
    }
}
