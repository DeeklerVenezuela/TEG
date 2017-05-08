using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Suscripcion
    {
        [Key]
        public int SuscripcionID { get; set; }
        public int UserID { get; set; }
        [MaxLength(128)]
        public string codigo { get; set; }
        public int VideoID { get; set; }
        public DateTime Fecha { get; set; }
        public virtual Video Videos { get; set; }
        public virtual User Users { get; set; }
        public bool Status { get; set; }

       
    }

}
