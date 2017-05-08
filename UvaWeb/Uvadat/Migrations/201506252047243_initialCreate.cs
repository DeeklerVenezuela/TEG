namespace UvaData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 50, storeType: "nvarchar"),
                        Apellido = c.String(maxLength: 50, storeType: "nvarchar"),
                        Email = c.String(maxLength: 50, storeType: "nvarchar"),
                        Cedula = c.String(maxLength: 10, storeType: "nvarchar"),
                        Password = c.String(maxLength: 150, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
