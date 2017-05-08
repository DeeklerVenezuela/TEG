using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class LikesController : Controller
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult AddLike(int videoId, int userId, bool tipo)
        {
            string[] result = { "false" };
            using (var db = new UvaContext())
            {
                var existe = db.Likes.Where(x => x.UserID == userId && x.VideoID == videoId).FirstOrDefault();
                if (existe == null)
                {
                    Like like = new Like();
                    like.Fecha = DateTime.Now;
                    like.Tipo = (tipo == true) ? true : false;
                    like.UserID = userId;
                    like.VideoID = videoId;
                    db.Likes.Add(like);
                    db.SaveChanges();
                    string[] great = { "true" };
                    return Json(great, JsonRequestBehavior.AllowGet);
                }
                if(existe != null && existe.Tipo != tipo)
                {
                    existe.Tipo = tipo;
                    db.Entry(existe).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    string[] great = { "true" };
                    return Json(great, JsonRequestBehavior.AllowGet);
                }
                    
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetLikes(int videoId)
        {
            int positive = 0;
            int negative = 0;
            int comentarios = 0;
            int reproducciones = 0;
            using (var db = new UvaContext())
            {
                positive = db.Likes.Where(x => x.Tipo == true && x.VideoID == videoId).Count();
                negative = db.Likes.Where(x => x.Tipo == false && x.VideoID == videoId).Count();
                reproducciones = db.Reproduccions.Where(x => x.VideoID == videoId).Count();
                comentarios = db.Comentarios.Where(x => x.VideoID == videoId).Count();
            }
            int[] result = { positive, negative, comentarios, reproducciones };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        
    }
}