using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    [Table("CarreraUser")]
    public class CarreraUser
    {
        [Key]
        public int CarreraID { get; set; }
        [MaxLength(50)]
        public string NombreCarrera { get; set; }
    }
}
