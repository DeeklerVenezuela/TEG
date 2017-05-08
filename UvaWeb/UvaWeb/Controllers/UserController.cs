using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;
namespace UvaWeb.Controllers
{
    public class UserController : Controller
    {
        private UvaContext db = new UvaContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUser(int id)
        {
            string nombre = null;
            string apellido = null;
            string foto = null;
            var user = db.Users.Where(x => x.UserID == id).FirstOrDefault<User>();
            if (user != null)
            {
                nombre = user.Nombre;
                apellido = user.Apellido;
                if (user.Foto == null)
                {
                    foto = "/videosunefa/img/user-placeholder.jpg";
                }
                else
                {
                    foto = user.Foto;
                }
            }
            string[] res = { nombre, apellido, foto };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsersPendientes(){
            IEnumerable<User> Users =  null;
            using(var db = new UvaContext()){
                Users = db.Users.Where(x=>x.Status == false && x.Type != 1).ToList();
            }
            var result = Users.Select(x=>new{Nombre = x.Nombre + " " + x.Apellido, Foto = (x.Foto == null) ? "/videosunefa/img/user-placeholder.jpg" : x.Foto, Id = x.UserID, Tipo = (x.Type == 2) ? "Docente" : "Administrador", Cedula = x.Cedula}).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult AprobarRegistro(int id)
        {
            using (var db = new UvaContext())
            {
                var user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                user.Status = true;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string[] result = { "Felicidades!!!", "Solicitud aprobada..." };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFoto(int id)
        {
            User user = new User();
            using (var db = new UvaContext())
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();

            string foto = "";
            if (user != null)
                foto = user.Foto;

            string[] result = { foto };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmail(int id)
        {
            User user = new User();
            using (var db = new UvaContext())
                user = db.Users.Where(x => x.UserID == id).FirstOrDefault();

            string email = "";
            string nombre = "";
            string apellido = "";
            if (user != null) {
                email = user.Email;
                nombre = user.Nombre;
                apellido = user.Apellido;
            }
                

            string[] result = { email, nombre, apellido };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateEmail(int id, string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                string[] bad = { "Disculpe!", "El email no puede estar vacío." };
                return Json(bad, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User user = new User();
                using (var db = new UvaContext())
                {
                    user = db.Users.Where(x => x.UserID == id).FirstOrDefault();

                    if (user != null)
                        user.Email = email;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            string[] result = { "Felicidades!!!", "Datos actualizados exitosamente." };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeContrasena(int id, string actual, string nuevo1, string nuevo2){
            if ((String.IsNullOrEmpty(actual)) || (String.IsNullOrEmpty(nuevo1)) || (String.IsNullOrEmpty(nuevo2)))
            {
                string[] vacios = { "Disculpe!", "Los campos no pueden estar vacío." };
                return Json(vacios, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (nuevo1 != nuevo2)
                {
                    string[] diferentes = { "Disculpe!", "Los las nuevas contraseñas no coinciden." };
                    return Json(diferentes, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    
                    var res = validarString(nuevo1);
                    if (res == nuevo1)
                    {
                        if (nuevo1.Length > 7 && nuevo1.Length < 16)
                        {
                            using (var db = new UvaContext())
                            {
                                var entity = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                                if (Repository.HashCode.ValidarClave(actual, entity.Password))
                                {
                                    entity.Password = Repository.HashCode.CalcularHash(nuevo1);
                                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    string[] incorrecta = { "Disculpe!", "Contraseña actual incorrecta." };
                                    return Json(incorrecta, JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                        else
                        {
                            string[] longitud = { "Disculpe!", "La contraseña debe contener entre 8 y 15 caracteres." };
                            return Json(longitud, JsonRequestBehavior.AllowGet);
                        }
                        
                    }
                    else
                    {
                        string[] especiales = { "Disculpe!", "La contraseña no puede contener caracteres especiales solo letras de la a-z A-Z y números." };
                        return Json(especiales, JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            string[] result = { "Felicidades!", "Contrseña cambiada exitosamente" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public static string validarString(string nuevo1)
        {
            Regex regex = new Regex(@"\w");
            string res = "";
            foreach (Match match in regex.Matches(nuevo1))
            {
                res = res + match.Value;
            }
            return res;
        }

        public JsonResult GetPreguntasDeSeguridad(string email)
        {
            Security entity = new Security();
            User user = new User();

            using (var db = new UvaContext())
            {
                user = db.Users.Where(x=>x.Email == email).FirstOrDefault();
            }
            if(user == null){
                string[] bad = {"false","Disculpe!","El email ingresado no se enuentra registrado."};
                return Json(bad,JsonRequestBehavior.AllowGet);
            }
            else{
                using(var dbc= new UvaContext()){
                     entity = db.Security.Where(x => x.User == user.UserID).FirstOrDefault();
                }
                if (entity != null)
                {
                    string[] result = { "true", entity.Q1, entity.Q2 };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] preg = { "false", "Disculpe!", "No se pueden encontrar sus preguntas de seguridad." };
                    return Json(preg, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        public JsonResult VerificarRespuestas(string email, string presp, string sresp)
        {
            Security entity = new Security();
            User user = new User();
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(presp) || String.IsNullOrEmpty(sresp))
            {
                string[] vacio = { "false", "Disculpe!", "Campos vacíos." };
                return Json(vacio, JsonRequestBehavior.AllowGet);
            }
            using (var db = new UvaContext())
            {
                user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            }
            if (user == null)
            {
                string[] bad = { "false", "Disculpe!", "El email ingresado no se enuentra registrado." };
                return Json(bad, JsonRequestBehavior.AllowGet);
            }
            
            else
            {
                using (var dbc = new UvaContext())
                {
                    entity = db.Security.Where(x => x.User == user.UserID).FirstOrDefault();
                }
                if (entity != null)
                {
                    if((Repository.HashCode.ValidarClave(presp.ToUpper(),entity.R1)) && (Repository.HashCode.ValidarClave(sresp.ToUpper(),entity.R2))){
                        string[] result = { "true"};
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string[] inco = { "false", "Disculpe!", "Sus respuestas son incorrectas." };
                        return Json(inco, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    string[] preg = { "false", "Disculpe!", "No se pueden encontrar sus preguntas de seguridad." };
                    return Json(preg, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult CambiarClave(string email, string pcont, string scont)
        {
            if (String.IsNullOrEmpty(pcont) || String.IsNullOrEmpty(scont))
            {
                string[] vacio = { "false", "Disculpe!", "Campos vacíos." };
                return Json(vacio, JsonRequestBehavior.AllowGet);
            }
            if (pcont != scont)
            {
                string[] inco = { "false", "Disculpe!", "Las contraseñas ingresadas no coinciden." };
                return Json(inco, JsonRequestBehavior.AllowGet);
            }
            if (pcont.Length < 8 || scont.Length > 15) 
            {
                string[] longitud = { "false","Disculpe!", "La contraseña debe contener entre 8 y 15 caracteres." };
                return Json(longitud, JsonRequestBehavior.AllowGet);
            }
            var res = validarString(pcont);
            if (res != pcont)
            {
                string[] especiales = { "false","Disculpe!", "La contraseña no puede contener caracteres especiales solo letras de la a-z A-Z y números." };
                return Json(especiales, JsonRequestBehavior.AllowGet);
            }
            User user = new User();
            using (var db = new UvaContext())
            {
                user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            }
            if (user == null)
            {
                string[] bad = { "false", "Disculpe!", "El email ingresado no se enuentra registrado." };
                return Json(bad, JsonRequestBehavior.AllowGet);
            }
            else
            {
                user.Password = Repository.HashCode.CalcularHash(pcont);
                using (var dbc = new UvaContext())
                {
                    dbc.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    dbc.SaveChanges();
                }
                string[] result = { "true", "Felicidades!", "Contraseña actualizada, ya puede utilizarla para ingresar al sistema." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}