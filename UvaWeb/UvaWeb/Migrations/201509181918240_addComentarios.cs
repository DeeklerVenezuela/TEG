namespace UvaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComentarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comentario",
                c => new
                    {
                        ComentarioID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        VideoID = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250, storeType: "nvarchar"),
                        Fecha = c.DateTime(nullable: false, precision: 0),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ComentarioID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comentario");
        }
    }
}
