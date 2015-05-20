namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPoint1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddPoints",
                c => new
                    {
                        AddPointId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Count = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.AddPointId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AddPoints");
        }
    }
}
