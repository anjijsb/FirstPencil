namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addLevel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attentions",
                c => new
                    {
                        AttentionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Remarks = c.String(),
                        CreateDate = c.DateTime(),
                        IsStick = c.Boolean(nullable: false, defaultValue: false),
                        IsActive = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.AttentionId);

            CreateTable(
                "dbo.AttentionRegisters",
                c => new
                    {
                        AttentionRegisterId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        AttentionId = c.Int(),
                        CreateDate = c.DateTime(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.AttentionRegisterId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Attentions", t => t.AttentionId)
                .Index(t => t.UserId)
                .Index(t => t.AttentionId);

            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        LevelId = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false, defaultValue: 0),
                        PointCount = c.Int(nullable: false, defaultValue: 0),
                        Name = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.LevelId);

        }

        public override void Down()
        {
            DropIndex("dbo.AttentionRegisters", new[] { "AttentionId" });
            DropIndex("dbo.AttentionRegisters", new[] { "UserId" });
            DropForeignKey("dbo.AttentionRegisters", "AttentionId", "dbo.Attentions");
            DropForeignKey("dbo.AttentionRegisters", "UserId", "dbo.Users");
            DropTable("dbo.Levels");
            DropTable("dbo.AttentionRegisters");
            DropTable("dbo.Attentions");
        }
    }
}
