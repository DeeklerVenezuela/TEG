using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Noticia
    {
        [Key]
        public int NoticiaID { get; set; }
        [MaxLength(60)]
        public string Nombre { get; set; }
        [MaxLength(250)]
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UserTypes { get; set; }
    }
}
