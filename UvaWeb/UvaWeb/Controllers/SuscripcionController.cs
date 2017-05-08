using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class SuscripcionController : Controller
    {
        UvaWeb.Models.UvaContext db = new Models.UvaContext(); //Contexto de la base de datos
        // GET: Suscripcion
        public ActionResult Index()
        {
            return View();
        }

        //Crear suscripcióna traves de un código 
        public JsonResult CreateSuscriptionWithCode(string code, int userId)
        {
            var estado = "Lo sentimos!";
            var result = "Error al suscribirse.";
            var getUserGroup = db.Unions.Where(x => x.UserID == userId).ToList(); //Buscar grupos del usuario
            if (getUserGroup != null)
            {
                var getTeacherId = db.Videos.Where(x => x.Codigo == code).FirstOrDefault();
                if (getTeacherId != null)
                {
                    int acum = 0;
                    foreach (var j in getUserGroup)
                    {
                        var g = db.Unions.Where(x => x.GrupoID == j.GrupoID && x.UserID == getTeacherId.UserID).FirstOrDefault();
                        if (g != null)
                            acum = acum + 1;
                    }
                    if (acum > 0)
                    {
                        var getVideo = db.Videos.Where(x => x.Codigo == code).FirstOrDefault();
                        if (getVideo != null)
                        {
                            Models.Suscripcion entity = new Models.Suscripcion();
                            entity.Fecha = DateTime.Now;
                            entity.UserID = userId;
                            entity.VideoID = getVideo.VideoID;
                            entity.Status = true;
                            if (entity != null)
                            {
                                var sus = VerificateSuscription(userId, getVideo.VideoID);
                                if (sus == true)
                                {
                                    CreateSuscription(entity);
                                    estado = "Felicidades!!!";
                                    result = "Suscripción agregada exitosamente.";
                                }
                                else
                                {
                                    result = "Usted ya tiene una suscripción agregada para este video.";
                                }
                                   
                            }    

                        }
                        
                    }
                        
                }
            }
            string[] res = { estado , result };
            return Json(res, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON
        }

        public  JsonResult CreateSuscriptionWithIA(string code, int userId)
        {
            var estado = "Lo sentimos!";
            var result = "Error al suscribirse.";
            var getUserGroup = db.Unions.Where(x => x.UserID == userId).ToList(); //Buscar grupos del usuario
            if (getUserGroup != null)
            {
                var getTeacherId = db.Videos.Where(x => x.Codigo == code).FirstOrDefault();
                if (getTeacherId != null)
                {
                    int acum = 0;
                    foreach (var j in getUserGroup)
                    {
                        var g = db.Unions.Where(x => x.GrupoID == j.GrupoID && x.UserID == getTeacherId.UserID).FirstOrDefault();
                        if (g != null)
                            acum = acum + 1;
                    }
                    if (acum > 0)
                    {
                        var getVideo = db.Videos.Where(x => x.Codigo == code).FirstOrDefault();
                        if (getVideo != null)
                        {
                            Models.Suscripcion entity = new Models.Suscripcion();
                            entity.Fecha = DateTime.Now;
                            entity.UserID = userId;
                            entity.VideoID = getVideo.VideoID;
                            entity.Status = false;
                            if (entity != null)
                            {
                                var sus = VerificateSuscription(userId, getVideo.VideoID);
                                if (sus == true)
                                {
                                    CreateSuscription(entity);
                                    estado = "Felicidades!!!";
                                    result = "Su solicitud ha sido enviada correctamente y se encuentra en espera de aprovación del docente.";
                                }
                                else
                                {
                                    result = "Usted ya tiene una suscripción agregada para este video.";
                                }

                            }

                        }

                    }

                }
            }
            string[] res = { estado, result };
            return Json(res, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON

        }
        private void CreateSuscription(Models.Suscripcion entity)
        {
            var insertQuery = db.Suscripcions.Add(entity);
            db.SaveChanges();
        }

        private bool VerificateSuscription(int UserId, int VideoId)
        {
            var result = true;
            var suscripcion = db.Suscripcions.Where(x => x.UserID == UserId && x.VideoID == VideoId).FirstOrDefault();
            if (suscripcion != null)
            {
                result = false;
            }
            return result;
        }

        public JsonResult BuscarSuscripcionesDisponibles(int id)
        {
            List<UvaWeb.Models.Video> videos = new List<Models.Video>();
            using (var dbc = new UvaWeb.Models.UvaContext())
            {
                var profesores = GruposController.BuscarProfesoresG(id);
                foreach (var i in profesores)
                {
                    var lsta = dbc.Videos.Where(x => x.UserID == i.UserID).ToList();
                    foreach (var j in lsta)
                    {
                        videos.Add(j);
                    }
                }
                var SusAct = dbc.Suscripcions
                    .Join(dbc.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus,vid)=>new{Sus = sus, Vid = vid}
                    ).Where(x => x.Sus.UserID == id).ToList();

                foreach (var k in SusAct)
                {
                    videos.Remove(k.Vid);
                }
            }

            var result = videos
                .Join(db.Users,
                vid => vid.UserID,
                us => us.UserID,
                (vid, us) => new { Vruta = vid.Thumb, Vnom = vid.Nombre, Vdesc = vid.Descripcion, Vcod = vid.Codigo, Unom = us.Nombre, Uape = us.Apellido });

            return Json(result, JsonRequestBehavior.AllowGet);       
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarSuscripcionesPendientes(int id)
        {

            List<VideoReturn> videos = null;
            using (var db = new UvaContext())
            {

                videos = (from us in db.Users
                          join vid in db.Videos on us.UserID equals vid.UserID
                          join sus in db.Suscripcions on vid.VideoID equals sus.VideoID
                          where sus.UserID == id && sus.Status == false
                          select new VideoReturn
                          {
                              VideoID = vid.VideoID,
                              UserID = us.UserID,
                              Nombre = vid.Nombre,
                              Descripcion = vid.Descripcion,
                              Fecha = vid.Fecha.Day + "/" + vid.Fecha.Month + "/" + vid.Fecha.Year,
                              URL = vid.URL,
                              Thumb = vid.Thumb,
                              NombreProfesor = us.Nombre,
                              ApellidoProfesor = us.Apellido,
                              Likes = vid.Likes.Count
                          }).ToList();
            }

            var js = new JavaScriptSerializer();
            var serializedResult = js.Serialize(videos);
            return Json(videos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSuscripcion(int videoId, int userId) {
            string[] result = { "Felicidades!!!", "Suscripción eliminada exitosamente." };
            using (var db = new UvaContext())
            {
                var entity = db.Suscripcions.Where(x => x.VideoID == videoId).FirstOrDefault();
                var mensajes = db.Mensajes.Where(x => x.UserID == userId && x.VideoID == videoId).ToList();
                if (mensajes != null)
                {
                    foreach (var i in mensajes)
                    {
                        db.Mensajes.Remove(i);
                    }
                }
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}