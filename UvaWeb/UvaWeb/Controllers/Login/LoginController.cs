using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;
using System.Web.Http.Cors;

namespace UvaWeb.Controllers.Login
{
    public class LoginController : Controller
    {
        private UvaContext uc = new UvaContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(FormCollection form)
        {
            string user = form["txtUser"];
            string pass = form["txtPass"];
            var a = uc.Users.Where(x => x.Email == user).FirstOrDefault<Models.User>();
            if (a == null)
            {
                ViewBag.Error = "Usuario o contraseña invalidos por favor verifique e intente de nuevo";
                return View("Index");
            }
            else
            {
                Session["name"] = a.Nombre;
                Session["apellido"] = a.Apellido;
                Session["type"] = a.Type;
                Session["id"] = a.UserID;
                return RedirectToAction("Index", "Home");
            }
               

        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult LoginMain(login login)
        {
            bool result = false;
            string email = login.email;
            string pass = login.pass;
            var a = uc.Users.Where(x => x.Email == email).FirstOrDefault<Models.User>();

            if (a != null)
            {
                if (a.Status == true)
                {
                    if (Repository.HashCode.ValidarClave(pass, a.Password))
                    {
                        var sesion = new Models.Session();
                        sesion.Fecha = DateTime.Now;
                        sesion.UserID = a.UserID;
                        uc.Sessions.Add(sesion);
                        uc.SaveChanges();
                        return Json(new { result = true, user = a.UserID , tipo = a.Type, mensaje = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, user = "", mensaje = "Usuario o contraseña inválido" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = false;
                }
                
            }
            return Json(new { result, mensaje = "Su cuenta se encuentra temporalmente bloqueada, por favor contacte a un administrador." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return RedirectToAction("Register", "Register");
        }
    }
}