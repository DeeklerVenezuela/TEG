using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvaWeb.Models
{
    public class UserAdd
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string Carrera { get; set; }
        public string Preg1 { get; set; }
        public string Resp1 { get; set; }
        public string Preg2 { get; set; }
        public string Resp2 { get; set; }
        public byte[] Foto { get; set; }
        public int Type { get; set; }

    }
}
