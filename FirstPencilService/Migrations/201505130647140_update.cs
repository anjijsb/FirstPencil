namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "SalesmanId", "dbo.Salesmen");
            DropIndex("dbo.Users", new[] { "SalesmanId" });
            AlterColumn("dbo.Users", "SalesmanId", c => c.Int());
            AddForeignKey("dbo.Users", "SalesmanId", "dbo.Salesmen", "SalesmanId");
            CreateIndex("dbo.Users", "SalesmanId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "SalesmanId" });
            DropForeignKey("dbo.Users", "SalesmanId", "dbo.Salesmen");
            AlterColumn("dbo.Users", "SalesmanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "SalesmanId");
            AddForeignKey("dbo.Users", "SalesmanId", "dbo.Salesmen", "SalesmanId", cascadeDelete: true);
        }
    }
}
