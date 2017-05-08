using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Entity;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace UvaModel
{
    public class User
    {
        
        public int UserID { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(50)]
        public string Apellido { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string Cedula { get; set; }
        
        [MaxLength(150)]
        public string Password { get; set; }

        public int Type { get; set; }
        
    }
}
