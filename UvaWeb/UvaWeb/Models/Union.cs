using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Union
    {
        [Key]
        public int InionID { get; set; }
        public int UserID { get; set; }
        public int GrupoID { get; set; }
        public DateTime Fecha { get; set; }
        public Grupo Grupo { get; set; }
        public User User { get; set; }
    }
}
