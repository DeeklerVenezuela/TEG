using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Grupo
    {
        [Key]
        public int GrupoID { get; set; }
        public int UserID { get; set; }
        [MaxLength(1)]
        public string Seccion { get; set; }
        public int Semestre { get; set; }
        [MaxLength(1)]
        public string Turno { get; set; }
        public DateTime Fecha { get; set; }
        public string Carrera { get; set; }
        public ICollection<Models.User> Users { get; set; }
    }
}
