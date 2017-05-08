using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Reproduccion
    {
        [Key]
        public int ReproduccionID { get; set; }

        public int VideoID { get; set; }

        public int UserID { get; set; }

        public DateTime Fecha { get; set; }
    }
}
