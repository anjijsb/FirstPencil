namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPrize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prizes",
                c => new
                    {
                        PrizeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Remarkes = c.String(),
                    })
                .PrimaryKey(t => t.PrizeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Prizes");
        }
    }
}
