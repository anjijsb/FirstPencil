namespace FirstPencilService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTicket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JsApiTickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        Ticket = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TicketId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JsApiTickets");
        }
    }
}
