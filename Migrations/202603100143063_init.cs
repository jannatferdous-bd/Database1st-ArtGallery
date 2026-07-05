namespace ArtGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayMethods",
                c => new
                    {
                        PayMethodId = c.Int(nullable: false, identity: true),
                        PayMethodName = c.String(),
                    })
                .PrimaryKey(t => t.PayMethodId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        SaleDate = c.DateTime(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        PayMethodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("dbo.PayMethods", t => t.PayMethodId, cascadeDelete: true)
                .Index(t => t.PayMethodId);
            
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        SaleDetailId = c.Int(nullable: false, identity: true),
                        ArtName = c.String(),
                        Quantity = c.Int(nullable: false),
                        SaleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleDetailId)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId);
            
            CreateTable(
                "dbo.tblRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        UserId = c.Int(nullable: false),
                        tblUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblUsers", t => t.tblUser_Id)
                .Index(t => t.tblUser_Id);
            
            CreateTable(
                "dbo.tblUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRoles", "tblUser_Id", "dbo.tblUsers");
            DropForeignKey("dbo.SaleDetails", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "PayMethodId", "dbo.PayMethods");
            DropIndex("dbo.tblRoles", new[] { "tblUser_Id" });
            DropIndex("dbo.SaleDetails", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "PayMethodId" });
            DropTable("dbo.tblUsers");
            DropTable("dbo.tblRoles");
            DropTable("dbo.SaleDetails");
            DropTable("dbo.Sales");
            DropTable("dbo.PayMethods");
        }
    }
}
