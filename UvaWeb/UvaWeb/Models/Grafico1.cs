using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Grafico1
    {
        public List<SeriesMain> SeriesMes { get; set; }
        public Serie Suscripciones { get; set; }
        public Serie Reproducciones { get; set; }
        public Serie Likes { get; set; }
        public Serie Dislikes { get; set; }
        public Serie Mensajes { get; set; }
        public Serie Sesiones { get; set; }
    }
}
