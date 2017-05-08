using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class Security
    {
        [Key]
        public int UserId { get; set; }
        public int User { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }
        
    }
}
