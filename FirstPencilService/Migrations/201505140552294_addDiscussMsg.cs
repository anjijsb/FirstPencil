namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscussMsg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscussMsgs",
                c => new
                    {
                        MsgId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Content = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MsgId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DiscussMsgs", new[] { "UserId" });
            DropForeignKey("dbo.DiscussMsgs", "UserId", "dbo.Users");
            DropTable("dbo.DiscussMsgs");
        }
    }
}
