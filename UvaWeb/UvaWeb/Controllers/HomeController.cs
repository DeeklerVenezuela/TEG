using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;
using UvaWeb.Controllers;
using System.Data.Entity;
using System.Security.Cryptography;
using NReco.VideoConverter;
using System.IO;

namespace UvaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //ViewBag.Name = (string)(Session["name"]);
            //ViewBag.Apellido = (string)(Session["apellido"]);
            //ViewBag.Type = int.Parse((Session["type"]).ToString());
            return View();
        }

        #region Suscripciones
        public ActionResult GetSuscriptions(JQueryDataTableParamModel param)
        {
            
            int user = 1;


            var a = new UvaWeb.Models.UvaContext();
               var result = (from s in a.Suscripcions
                          join v in a.Videos on s.VideoID equals v.VideoID
                          join u in a.Users on v.UserID equals u.UserID
                          where s.UserID == user
                          select new
                          {
                              CODE = s.codigo,
                              VIDEO = v.URL,
                              VIDEONAME = v.Nombre,
                              PROFESOR = u.Nombre + " " + u.Apellido,
                              STATUS = v.Status
                          }).ToList();
               
                var r = from d in result
                        select new string[] { d.CODE, "<a href="+d.VIDEO+">"+d.VIDEONAME+"</a>", d.PROFESOR, d.STATUS == true ? "<span class='mif-play fg-green margin10 no-margin-top no-margin-bottom'></span>" : "<span class='mif-stop fg-red no-margin-top no-margin-bottom margin10'></span>" };
 
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = result.Count(),
                aaData = r
            },
        JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSuscription(int dato)
        {
            using (var context = new UvaContext())
            {
                var wu = context.Suscripcions.Where(s => s.codigo == dato.ToString()).FirstOrDefault();
                context.Entry(wu).State = System.Data.Entity.EntityState.Deleted;
                var result = context.SaveChanges() > 0;
            }
            return Json("suscripcion eliminada exitosamente.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateSuscripcion(int dato)
        {
            Suscripcion s = new Models.Suscripcion();
            string result = "Error al suscribirse";
            using (var context = new UvaContext())
            {
                var bytes = new byte[4];
                Models.Suscripcion e = null;
                var rng = RandomNumberGenerator.Create();
                rng.GetBytes(bytes);
                uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
                s.codigo =String.Format("{0:D8}", random);
                s.Fecha = DateTime.Now;
                s.UserID = 1; //Session userid
                var vid = (from d in context.Videos
                           where d.Codigo == dato.ToString()
                           select d.VideoID).FirstOrDefault();
                s.VideoID = vid;
                e = context.Suscripcions.Where(z => z.VideoID == vid).FirstOrDefault();
                if(e == null)
                {
                    context.Suscripcions.Add(s);
                    context.SaveChanges();
                    result = "Suscripcion agregada exitosamente";
                }
                else{
                    result ="Error (ya se encuentra suscrito a este video)";
                }
                
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Grupos
        
       
        public JsonResult CreateGroup(Grupo grupo)
        {
            Grupo g = new Models.Grupo();
            string result = "Error al crear grupo";
            using (var context = new UvaContext())
            {
                g.Seccion = grupo.Seccion;
                g.Semestre = grupo.Semestre;
                g.Turno = grupo.Turno;
                g.UserID = 1;//cambiar po usuario de sesion
                g.Fecha = DateTime.Now;

                var exist = (from e in context.Grupos
                                 where e.UserID == 1
                                 select e).ToList();//cambiar po usuario de sesion
                
                var us = (from h in context.Users
                              where h.UserID == 1
                              select h.Type).FirstOrDefault();

                if (exist.Count() <= 2 && us == 2)
                    context.Grupos.Add(g);
                if (exist.Count() > 2 && us == 2)
                    result = "Error(Ha alcanzado el maximo de suscripciones a grupos)";
                if(us == 1)
                    context.Grupos.Add(g);
                if(exist.Count() <= 4 && us == 3)
                    context.Grupos.Add(g);
                if (exist.Count() > 4 && us == 3)
                    result = "Error(Ha alcanzado el maximo de suscripciones a grupos)";
                context.SaveChanges();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UnionGroup(string carrera, string seccion, int semestre, string turno)
        {
            string result = "Union a grupo";
            using (var context = new UvaContext())
            {               
                Models.Union exist = null;

                int grupo = (from t in context.Grupos
                             where t.Carrera == carrera
                             && t.Seccion == seccion
                             && t.Semestre == semestre
                             && t.Turno == turno
                             select t.GrupoID).FirstOrDefault();
                
                exist = (from e in context.Unions
                             where e.UserID == 1 
                             && e.GrupoID == grupo
                             select e).FirstOrDefault();

                if (exist == null)
                {
                    var union = new Models.Union();
                    union.UserID = 1; //cambiar por usuario de sesion
                    union.Fecha = DateTime.Now;
                    union.GrupoID = grupo;
                    context.Unions.Add(union);
                    context.SaveChanges();
                    result = "Su solicitud ha sido procesada exitosamente";
                }
                else
                    result = "Usted ya se encuentra suscrito a este grupo";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetGroups(JQueryDataTableParamModel param)
        //{
        //    //Cambiar 
        //    int user = 1;//User de la sesion


        //    var a = new UvaWeb.Models.UvaContext();
        //    var result = (from g in a.Grupos
        //                  join u in a.Users on g.UserID equals u.UserID
        //                  join un in a.Unions on g.GrupoID equals un.GrupoID
        //                  join c in a.Carreras on u.UserID equals c.UserID
        //                  where un.UserID == user && g.GrupoID == un.GrupoID
        //                  select new
        //                  {
        //                      CARRERA = c.NombreCarrera,
        //                      SECCION = g.Seccion,
        //                      SEMESTRE = g.Semestre,
        //                      TURNO = g.Turno
        //                  }).ToList();

        //    var r = from d in result
        //            select new string[] { d.CARRERA, d.SECCION, d.SEMESTRE.ToString(), d.TURNO };

        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = result.Count(),
        //        iTotalDisplayRecords = result.Count(),
        //        aaData = r
        //    },
        //JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetGroups(JQueryDataTableParamModel param)
        {
            //Cambiar 
            //int user = 1;//User de la sesion


            //var a = new UvaWeb.Models.UvaContext();
            //var result = (from g in a.Grupos
            //              join u in a.Users on g.UserID equals u.UserID
            //              join c in a.Carreras on u.UserID equals c.UserID
            //              where g.UserID == user
            //              select new
            //              {
            //                  CARRERA = c.NombreCarrera,
            //                  SECCION = g.Seccion,
            //                  SEMESTRE = g.Semestre,
            //                  TURNO = g.Turno
            //              }).ToList();

            var r =  new string[] { "Sistemas", "A", "7", "D" };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = 1,
                iTotalDisplayRecords = 1,
                aaData = r
            },
        JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSectionsInformation(int semestre, string turno)
        {
            var a = new UvaWeb.Models.UvaContext();
            var result = (from g in a.Grupos
                          where g.Semestre == semestre 
                          && g.Turno == turno
                          select new 
                          {
                              OPCION = g.Seccion
                          }).ToList();

            var r = from d in result
                    select new string[] {d.OPCION};

            return Json(r,JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Videos
        public ActionResult Subir(FormCollection form, HttpPostedFileBase Video)
        {
            if (Video.ContentLength > 0)
            {
                string pic = System.IO.Path.GetFileName(Video.FileName);
                //string userPath = (Session["id"]).ToString();
                string rn = System.IO.Path.GetFileNameWithoutExtension(Video.FileName);
                string userPath = "1";
                var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
                string path = System.IO.Path.Combine(Server.MapPath("~/Img"), pic);
                string completePath = "C://VideosUNEFA//" + userPath + "//";
                if (!Directory.Exists(completePath))
                {
                    Directory.CreateDirectory(completePath);
                }
                string path2 = completePath + pic;
                Video.SaveAs(path2);
                ffmpeg.ConvertMedia(completePath + pic, completePath + rn + ".mp4", Format.mp4);
                System.IO.File.Delete(completePath + pic);
                ViewBag.response = "cargado";
                ViewBag.path = completePath + rn + ".mp4";
                //var entity = new Models.Video();
                //entity.Descripcion = form["Descripcion"];
                //using (var ctx = new UvaWeb.Models.UvaContext())
                //{
                //    ctx.Videos.Add(entity);
                //}
            }



            return View("Index");
        }


        #endregion
    }
}
