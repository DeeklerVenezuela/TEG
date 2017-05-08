namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_extension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Video", "Extension", c => c.String(maxLength: 5, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Video", "Extension");
        }
    }
}
