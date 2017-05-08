using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class VideoEstadistico
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Reproducciones { get; set; }
        public string Thumb { get; set; }
        public string Fecha { get; set; }
        public int Suscripciones { get; set; }
    }
}
