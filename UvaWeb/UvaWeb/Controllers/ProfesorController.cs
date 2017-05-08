using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class ProfesorController : Controller
    {
        public JsonResult AprobarSuscripcionesPendintes(int suscripcionId, int userId)
        {
            User user = new User();
            using (var db = new UvaContext())
            {
                user = db.Users.Where(x => x.UserID == userId).FirstOrDefault();
            }
            if (user != null)
            {
                if (user.Type != 2)
                {
                    string[] badUser = { "Lo Sentimos!!!", "Solo los docentes pueden aprobar suscripciones." };
                    return Json(badUser, JsonRequestBehavior.AllowGet);
                }

                using (var db = new UvaContext())
                {
                    var suscripcion = db.Suscripcions.Where(x => x.SuscripcionID == suscripcionId).FirstOrDefault();
                    suscripcion.Status = true;
                    db.Entry(suscripcion).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    string[] result = { "Felicidades!!!", "Suscripción aprobada." };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            string[] badResult = { "Lo Sentimos!!!", "No puede aprobar la suscripción seleccionada en este momento." };
            return Json(badResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarSuscripcionesSinAprobar(int id)
        {
            
            using (var db = new UvaContext())
            {
                //var suscripciones = db.Suscripcions
                //    .Join(db.Users,
                //    sus => sus.UserID,
                //    usu => usu.UserID,
                //    (sus, usu) => new { Sus = sus, Usu = usu })
                //    .Join(db.Videos,
                //    sc => sc.Sus.VideoID,
                //    vid => vid.VideoID,
                //    (sc, vid) => new { Sc = sc, Vid = vid })
                //    .Join(db.Unions,
                //    suc => suc.Sc.Usu.UserID,
                //    uni => uni.UserID,
                //    (suc, uni) => new { Suc = suc, Uni = uni})
                //    .Where(z => z.Suc.Vid.UserID == id && z.Suc.Sc.Sus.Status == false).ToList();

                var suscripciones = db.Suscripcions
                    .Join(db.Users,
                    sus => sus.UserID,
                    usu => usu.UserID,
                    (sus, usu) => new { Sus = sus, Usu = usu })
                    .Join(db.Videos,
                    sc => sc.Sus.VideoID,
                    vid => vid.VideoID,
                    (sc, vid) => new { Sc = sc, Vid = vid })
                    .Where(z => z.Vid.UserID== id && z.Sc.Sus.Status == false).ToList();

                List<Union> uniones = new List<Union>();
                foreach (var t in suscripciones)
                {
                    var us = t.Sc.Usu.UserID;
                    var unip = db.Unions.Where(x => x.UserID == us).ToList();
                    foreach (var f in unip)
                    {
                        uniones.Add(f);
                    } 
                }
                //var uniones = suscripciones.Select(y => y.Uni).ToList();
                List<Grupo> gru = new List<Grupo>();
                foreach (var j in uniones)
                {
                    var grup = db.Grupos.Where(x => x.GrupoID == j.GrupoID).FirstOrDefault();
                    gru.Add(grup);
                }
                List<string> grupos = new List<string>();
                foreach (var i in gru)
                {
                    grupos.Add(i.Semestre + " | " + i.Carrera + " | " + i.Seccion + " | " + i.Turno);   
                }
                   var res = suscripciones.Select(y => new
                    {
                        //VideoID = y.Suc.Vid.VideoID,
                        //UserID = y.Suc.Sc.Usu.UserID,
                        //Nombre = y.Suc.Vid.Nombre,
                        //Fecha = y.Suc.Sc.Sus.Fecha.Day + "/" + y.Suc.Sc.Sus.Fecha.Month + "/" + y.Suc.Sc.Sus.Fecha.Year,
                        //Thumb = y.Suc.Sc.Usu.Foto,
                        //NombreProfesor = y.Suc.Sc.Usu.Nombre,
                        //ApellidoProfesor = y.Suc.Sc.Usu.Apellido,
                        //Cedula = y.Suc.Sc.Usu.Cedula,
                        //Grupos = grupos
                        VideoID = y.Vid.VideoID,
                        UserID = y.Sc.Usu.UserID,
                        Nombre = y.Vid.Nombre,
                        Fecha = y.Sc.Sus.Fecha.Day + "/" + y.Sc.Sus.Fecha.Month + "/" + y.Sc.Sus.Fecha.Year,
                        Thumb = y.Sc.Usu.Foto,
                        NombreProfesor = y.Sc.Usu.Nombre,
                        ApellidoProfesor = y.Sc.Usu.Apellido,
                        Cedula = y.Sc.Usu.Cedula,
                        Suscripcion = y.Sc.Sus.SuscripcionID,
                        Grupos = grupos
                    });
                   return Json(res, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}