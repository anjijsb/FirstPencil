namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        OpenId = c.String(),
                        NickName = c.String(),
                        subscribe = c.Boolean(nullable: true, defaultValue: false),
                        Sex = c.Int(nullable: true, defaultValue: 0),
                        City = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        Language = c.String(),
                        SubscribeTime = c.DateTime(),
                        Headimgurl = c.String(),
                        IsSalesman = c.Boolean(nullable: true, defaultValue: false),
                        SalesmanId = c.Int(nullable: true),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Salesmen", t => t.SalesmanId, cascadeDelete: true)
                .Index(t => t.SalesmanId);

            CreateTable(
                "dbo.Salesmen",
                c => new
                    {
                        SalesmanId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Sex = c.Int(nullable: true, defaultValue: 0),
                        FirmId = c.Int(),
                        Position = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.SalesmanId)
                .ForeignKey("dbo.Firms", t => t.FirmId)
                .Index(t => t.FirmId);

            CreateTable(
                "dbo.Firms",
                c => new
                    {
                        FirmId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Area = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.FirmId);

            CreateTable(
                "dbo.Complains",
                c => new
                    {
                        ComplainId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.Int(),
                        Title = c.String(),
                        Content = c.String(),
                        Address = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ComplainId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.ScanEvents",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventKey = c.String(),
                        IsActive = c.Boolean(),
                        CodeContent = c.String(),
                        CodeUrl = c.String(),
                        Type = c.Int(nullable: true, defaultValue: 0),
                        CodeType = c.Int(nullable: true, defaultValue: 0),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.EventId);

            CreateTable(
                "dbo.AccessTokens",
                c => new
                    {
                        AcceccTokenId = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        GetTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.AcceccTokenId);

        }

        public override void Down()
        {
            DropIndex("dbo.Complains", new[] { "UserId" });
            DropIndex("dbo.Salesmen", new[] { "FirmId" });
            DropIndex("dbo.Users", new[] { "SalesmanId" });
            DropForeignKey("dbo.Complains", "UserId", "dbo.Users");
            DropForeignKey("dbo.Salesmen", "FirmId", "dbo.Firms");
            DropForeignKey("dbo.Users", "SalesmanId", "dbo.Salesmen");
            DropTable("dbo.AccessTokens");
            DropTable("dbo.ScanEvents");
            DropTable("dbo.Complains");
            DropTable("dbo.Firms");
            DropTable("dbo.Salesmen");
            DropTable("dbo.Users");
        }
    }
}
