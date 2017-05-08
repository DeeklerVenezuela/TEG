using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        public int VideoID { get; set; }
        
        [MaxLength(35)]
        public string Etiqueta { get; set; }
    }
}
