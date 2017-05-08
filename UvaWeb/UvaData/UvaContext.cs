using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using MySql.Data.Entity;

namespace UvaData
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class UvaContext : DbContext
    {
        public UvaContext(){
            this.Configuration.ProxyCreationEnabled = false;
        }
        
        public DbSet<User> Users { get; set; }
    }
    
}
