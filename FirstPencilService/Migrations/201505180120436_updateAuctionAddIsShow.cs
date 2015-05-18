namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAuctionAddIsShow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "IsShow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Auctions", "IsShow");
        }
    }
}
