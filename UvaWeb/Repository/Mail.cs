using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Mail
    {
        public static string SendMail(string email, string nombre, string apellido, string cedula)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            
            mail.From = new MailAddress("activartegcuenta@gmail.com","Teg Jean Robles");
            mail.To.Add(new MailAddress("activartegcuenta@gmail.com"));
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("activartegcuenta@gmail.com", "unefa*2015");
            client.Host = "smtp.gmail.com";
            
            mail.Subject = "Verificación de cuenta (" + nombre + " " + apellido + ").";
            mail.IsBodyHtml = true;
            mail.Body = "<html lang='Es-es'><head><meta charset='utf-8'><title>Verificación de cuenta</title></head><body><h1>Verificación de cuenta</h1><p>El usuario" + nombre + " " + apellido+" solicita activar su cuenta</p><br><br><h2>Datos:</h2><br><p>Nombre:"+nombre+"<br>Apellido: "+apellido+"<br>Email: "+email+"<br>Cedula: "+cedula+"<br><br></p><br><p>Para activar esta cuenta por favor haga click en el siguiente enlace <a href='http://localhost:20539/Register/ValidarRegistro&'"+email+">Activar Cuenta</a></p></body></html>";
            client.Send(mail);
            return "Se ha enviado el correo de verificación";
        }
    }
}
