using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Video
    {
        [Key]
        public int VideoID { get; set; }

        [MaxLength(128)]
        public string Codigo { get; set; }
        
        [MaxLength(128)]
        public string URL { get; set; }
        [MaxLength(128)]
        public string Thumb { get; set; }

        [MaxLength(5)]
        public string Extension { get; set; }

        [MaxLength(64)]
        public string Nombre { get; set; }

        [MaxLength(250)]
        public string Descripcion { get; set; }

        public int UserID { get; set; }

        public bool Status { get; set; }

        public int Semestre { get; set; }

        public DateTime Fecha { get; set; }

        public  virtual List<Suscripcion> Suscripciones { get; set; }

        public  virtual List<Reproduccion> Reproducciones { get; set; }

        public virtual List<Like> Likes { get; set; }

        public  virtual List<Tag> Tags { get; set; }

        public  virtual List<Mensaje> Mensajes { get; set; }



    }
}
