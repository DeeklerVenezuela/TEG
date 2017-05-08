using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class SemestreUser
    {
        [Key]
        public int UserID { get; set; }
        public int Semestre { get; set; }
        public bool Status { get; set; }
        public DateTime Fecha { get; set; }
    }
}
