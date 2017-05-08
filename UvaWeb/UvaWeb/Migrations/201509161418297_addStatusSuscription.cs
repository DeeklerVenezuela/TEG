namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatusSuscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suscripcion", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suscripcion", "Status");
        }
    }
}
