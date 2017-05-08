namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifySecurity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Security", "User", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Security", "User");
        }
    }
}
