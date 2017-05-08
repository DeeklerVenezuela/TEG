namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifySession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Session", "Fecha", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.Session", "Apertura");
            DropColumn("dbo.Session", "Cierre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Session", "Cierre", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("dbo.Session", "Apertura", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.Session", "Fecha");
        }
    }
}
