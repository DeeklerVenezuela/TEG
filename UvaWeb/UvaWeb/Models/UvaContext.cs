using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using UvaWeb.Models;
using MySql.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace UvaWeb.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class UvaContext : DbContext
    {
        public UvaContext(): base("UvaContext")
        {
            //Database.SetInitializer<UvaContext>(new UvaContextInitalizator());
            Database.SetInitializer<UvaContext>(new CreateDatabaseIfNotExists<UvaContext>());
        }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Reproduccion> Reproduccions { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Estadistica> Estadisticas { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SemestreUser> SemestreUsers { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Suscripcion> Suscripcions { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<CarreraUser> CarrerasUsers { get; set; }
        public DbSet<Union> Unions { get; set; }
        public DbSet<CuentasVerificar> CuentasVerificars { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Chat> Chats { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
