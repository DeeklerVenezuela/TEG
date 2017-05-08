using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class VideoReturn
    {
        public string Codigo { get; set; }
        public string Cedula { get; set; }
        public int VideoID{get; set;}
        public int UserID { get; set; }
        public string Nombre { get; set; } 
        public string Descripcion {get; set;} 
        public int Likes {get; set;}
        public string Fecha { get; set; }
        public string URL{get; set;}
        public string Thumb {get; set;}
        public string NombreProfesor { get; set; }
        public string ApellidoProfesor { get; set; }
        public List<string> Grupos { get; set; }
        public bool Status { get; set; }
    }
}
