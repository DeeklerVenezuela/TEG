using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NReco.VideoConverter;
using System.Security.Cryptography;
using UvaWeb.Models;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Collections;


namespace UvaWeb.Controllers.Videos
{
    public class VideosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subir(FormCollection form, HttpPostedFileBase Video)
        {
            if(Video.ContentLength > 0)
            {
                string pic = System.IO.Path.GetFileName(Video.FileName);
                string rn = System.IO.Path.GetFileNameWithoutExtension(Video.FileName);
                string userPath = "1";
                var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
                string path = System.IO.Path.Combine(Server.MapPath("~/Img"), pic);
                string completePath = "C://VideosUNEFA//" + userPath +"//";
                if (!Directory.Exists(completePath))
                {
                    Directory.CreateDirectory(completePath);
                }
                string path2 = completePath + pic;
                Video.SaveAs(path2);
                ffmpeg.ConvertMedia(completePath + pic, completePath + rn + ".mp4", Format.mp4);
                System.IO.File.Delete(completePath + pic);
                ViewBag.response = "cargado";
                ViewBag.path = completePath+rn+".mp4";
                
            }
            
                

            return View("Index");
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarVideosSuscritos(Models.UserSearch user)
        {
            
            List<VideoReturn> videos = null;
            using (var db = new UvaContext())
            {
                
                videos = (from us in db.Users
                          join vid in db.Videos on us.UserID equals vid.UserID
                          join sus in db.Suscripcions on vid.VideoID equals sus.VideoID
                          where sus.UserID == user.id && sus.Status == true && vid.Status == true
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
            return Json(videos , JsonRequestBehavior.AllowGet);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult RegistrarReproduccion(int videoId, int userId)
        {
            using (var db = new UvaContext())
            {
                Reproduccion re = new Reproduccion();
                re.Fecha = DateTime.Now;
                re.UserID = userId;
                re.VideoID = videoId;
                db.Reproduccions.Add(re);
                db.SaveChanges();
            }
            string[] great = { "Felicidades!!!", "Reproducción agregada" };
            return Json(great, JsonRequestBehavior.AllowGet);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarVideosPorID(int id)
        {

            VideoReturn video = null;
            using (var db = new UvaContext())
            {
                
                video = (from us in db.Users
                          join vid in db.Videos on us.UserID equals vid.UserID
                          where vid.VideoID == id
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
                              Likes = vid.Likes.Count,
                              Status = vid.Status
                          }).FirstOrDefault();
            }

            var js = new JavaScriptSerializer();
            var serializedResult = js.Serialize(video);
            return Json(video, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllVideos(int id)
        {
            List<VideoReturn> videos = new List<VideoReturn>();
            using (var db = new UvaContext())
            {

                videos = (from us in db.Users
                         join vid in db.Videos on us.UserID equals vid.UserID
                         where vid.UserID == id
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
                             Likes = vid.Likes.Count,
                             Codigo = vid.Codigo,
                             Status = vid.Status
                         }).OrderByDescending(x=>x.Fecha).ToList();
            }
            return Json(videos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateVideo(int id, string nombre, string descripcion, bool status)
        {
            using (var db = new UvaContext())
            {
                var entity = db.Videos.Where(x => x.VideoID == id).FirstOrDefault();
                entity.Nombre = nombre;
                entity.Descripcion = descripcion;
                entity.Status = status;
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            string[] result = { "Felicidades", "Los datos de su video fueron actualizados" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
    }
}