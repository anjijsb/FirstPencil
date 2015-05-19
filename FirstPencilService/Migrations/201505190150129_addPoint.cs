namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Point", c => c.Int());
            AddColumn("dbo.Complains", "ImgId", c => c.String());
            AddColumn("dbo.Complains", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Complains", "ImgPath");
            DropColumn("dbo.Complains", "ImgId");
            DropColumn("dbo.Users", "Point");
        }
    }
}
