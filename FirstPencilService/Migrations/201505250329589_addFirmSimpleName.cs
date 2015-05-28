namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFirmSimpleName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Firms", "SimpleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Firms", "SimpleName");
        }
    }
}
