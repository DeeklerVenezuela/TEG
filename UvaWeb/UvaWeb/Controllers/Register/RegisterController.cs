using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers.Register
{
    public class RegisterController : Controller
    {
        
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult ValidarRegistro(string email)
        {
            Models.User selection;
            using(var db = new UvaContext()){
                selection = db.Users.Where(x => x.Email == email).FirstOrDefault();//Buscar registro
            }
            if (selection != null)
            {
                selection.Status = true;//Actualizar Registro
            }
            using (var dbCtx = new UvaContext())
            {
                dbCtx.Entry(selection).State = System.Data.Entity.EntityState.Modified;//Guardar Status

                dbCtx.SaveChanges();
            }

            return Json(new { result = "Cuenta activada correctamente" }, JsonRequestBehavior.AllowGet);
        }




        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult Register(UserAdd user)
        {
            string respuesta = "";
            string Pass = user.Password ;
            string Pass2 = user.Password2;
            if (!String.IsNullOrEmpty(Pass) && !String.IsNullOrEmpty(Pass2))
            {
                if (Pass == Pass2)
                {
                    if (Pass.Length > 7 && Pass.Length < 16)
                    {
                        var res = UserController.validarString(Pass);
                        if(res == Pass){
                            var db = new UvaContext();
                            var carreraId = db.CarrerasUsers.Where(x => x.NombreCarrera == user.Carrera).Select(y => y.CarreraID).FirstOrDefault();
                            User entity = new Models.User();
                            entity.Type = user.Type;
                            entity.Nombre = user.Nombre;
                            entity.Apellido = user.Apellido;
                            entity.Cedula = user.Cedula;
                            entity.Email = user.Email;
                            entity.CarreraId = carreraId;
                            if (user.Type == 1)
                            {
                                entity.Status = true;
                            }
                            else
                            {
                                entity.Status = false;
                                CuentasVerificar cpv = new CuentasVerificar();
                                cpv.Nombre = entity.Nombre;
                                cpv.Apellido = entity.Apellido;
                                cpv.Cedula = entity.Cedula;
                                cpv.Email = entity.Email;
                                cpv.Fecha = DateTime.Now;
                                cpv.Type = entity.Type;
                                db.CuentasVerificars.Add(cpv);
                                db.SaveChanges();
                            }
                            //entity.Password = user.Password; 
                            entity.Password = Repository.HashCode.CalcularHash(user.Password);

                            //Agregar  a base de datos
                            var existe = db.Users.Where(x => x.Email == entity.Email).FirstOrDefault();
                            if (existe != null)
                            {
                                if (existe.Email == user.Email)
                                {
                                    respuesta = "El email ingresado ya se encuentra registrado, por favor ingrese otro";
                                }
                            }
                            else
                            {

                                db.Users.Add(entity);
                                db.SaveChanges();

                                using (var dbn = new UvaContext()) //Agregar preguntas de seguridad
                                {
                                    string pr1 = user.Resp1.ToUpper();
                                    string pr2 = user.Resp2.ToUpper();
                                    Security sc = new Security();
                                    sc.Q1 = user.Preg1;
                                    sc.Q2 = user.Preg2;
                                    sc.R1 = Repository.HashCode.CalcularHash(pr1);
                                    sc.R2 = Repository.HashCode.CalcularHash(pr2);
                                    sc.User = dbn.Users.Where(x => x.Email == user.Email).Select(y => y.UserID).FirstOrDefault();
                                    dbn.Security.Add(sc);
                                    dbn.SaveChanges();
                                }
                                //Fin Agregar
                                respuesta = "Felicidades!!! Tu cuenta ha sido creada exitosamente";
                            }

                        }
                        else
                        {
                            respuesta = "La contraseña no puede contener caracteres especiales solo letras de la a-z A-Z y números.";
                        }
                        
                    }
                    else
                    {
                        respuesta = "La contraseña debe contener entre 8 y 15 caracteres.";
                    }                      
                }
                else
                {
                    respuesta = "Contraseñas no coinciden";
                }
            }
            return Json(respuesta,JsonRequestBehavior.AllowGet);
        }

       
    }
}