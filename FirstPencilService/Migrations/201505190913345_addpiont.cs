namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addpiont : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prizes", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Prizes", "IsActive");
        }
    }
}
