using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Data;
using System.Data.Entity;
using MySql.Data.Entity;
using UvaModel;

namespace UvaData
{
    public class UvaContext : DbContext
    {
        public UvaContext()
            : base("UvaContext")
            {
                Database.SetInitializer<UvaContext>(new CreateDatabaseIfNotExists<UvaContext>());
            }
            public DbSet<User> Users { get; set; }
        
    }
}
