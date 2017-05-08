using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class ConfigController : Controller
    {
        private UvaContext db = new UvaContext();
        // GET: Config
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult BasicConfig(string Nombre, string Apellido, string Email, int UserId)
        {
            User user = new User();
            string estado = "Lo sentimos!";
            string resultado = "Error al actualizar sus datos";
            using (var dbc = db)
            {
                user = db.Users.Where(x => x.UserID == UserId).FirstOrDefault<User>();
            }

            if (user != null)
            {
                user.Nombre = Nombre;
                user.Apellido = Apellido;
                user.Email = Email;
            }

            using (var dbCtx = new UvaContext())
            {
                dbCtx.Entry(user).State = System.Data.Entity.EntityState.Modified;
                dbCtx.SaveChanges();
                estado = "Felicidades!!!";
                resultado = "Datos actualizados";
            }

            string[] res = { estado, resultado };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PassConfig(string OldPass, string NewPass1, string newPass2, int UserId)
        {
            User user = new User();
            string resultado = "Error al actualizar contraseña";
            string estado = "Lo Sentimos!";
            using (var dbc = db)
            {
                user = dbc.Users.Where(x => x.UserID == UserId).FirstOrDefault<User>();
            }

            if (user != null)
            {
                if (OldPass == user.Password)
                {
                    if (NewPass1 == newPass2)
                    {
                        user.Password = NewPass1;
                        resultado = "Contraseña actiualizada correctamente";
                        estado = "Felicidades!!!";
                    }
                    else
                    {
                        resultado = "Las contraseñas nuevas ingresadas no coinciden";
                    }
                }
                else
                {
                    resultado = "Contraseña anterior incorrecta";
                }
            }

            using (var dbCtx = new UvaContext())
            {
                dbCtx.Entry(user).State = System.Data.Entity.EntityState.Modified;
                dbCtx.SaveChanges();
            }

            string[] res = {estado,resultado};
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult QuestionsConfig(string Q1, string R1, string Q2, string R2, int UserId) 
        {
            string resultado = "Error al actualizar preguntas de seguridad";
            string estado = "Lo Sentimos!";
            using (var dbc = db)
            {
                var user = dbc.Users.Join(
                    dbc.Security,
                    usr => usr.UserID,
                    sec => sec.UserId,
                    (usr, sec) => new { US = usr.UserID, SC = sec }).Where(x => x.US == UserId).FirstOrDefault();

                if (user != null)
                {
                    user.SC.Q1 = Q1;
                    user.SC.Q2 = Q2;
                    user.SC.R1 = R1;
                    user.SC.R2 = R2;
                    estado = "Felicidades!!!";
                    resultado = "Preguntas de seguridad actualizadas";
                }

                dbc.Entry(user).State = System.Data.Entity.EntityState.Modified;
                dbc.SaveChanges();
            }
            string[] res = { estado, resultado };
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}