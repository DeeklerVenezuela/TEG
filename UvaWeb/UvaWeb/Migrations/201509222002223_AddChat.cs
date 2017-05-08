namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ChatID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        VideoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatID);
            
            AddColumn("dbo.Mensaje", "Chat", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mensaje", "Chat");
            DropTable("dbo.Chat");
        }
    }
}
