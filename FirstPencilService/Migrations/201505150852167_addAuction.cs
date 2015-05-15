namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAuction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        AuctionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PrductName = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Price = c.Double(),
                        Count = c.Int(),
                        TotalCount = c.Int(),
                        LimitCount = c.Int(),
                        TitleImg = c.String(),
                        Descripiton = c.String(),
                        DescribeImgs = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.AuctionId);
            
            CreateTable(
                "dbo.AuctionOrders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        AuctionId = c.Int(),
                        UserId = c.Int(),
                        Price = c.Double(),
                        Count = c.Int(),
                        CreatrDate = c.DateTime(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Auctions", t => t.AuctionId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.AuctionId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AuctionOrders", new[] { "UserId" });
            DropIndex("dbo.AuctionOrders", new[] { "AuctionId" });
            DropForeignKey("dbo.AuctionOrders", "UserId", "dbo.Users");
            DropForeignKey("dbo.AuctionOrders", "AuctionId", "dbo.Auctions");
            DropTable("dbo.AuctionOrders");
            DropTable("dbo.Auctions");
        }
    }
}
