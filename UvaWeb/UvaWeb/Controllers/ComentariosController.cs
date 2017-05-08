using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class ComentariosController : Controller
    {
        public JsonResult AddComentario(int userId, int videoId, string descripcion)
        {
            Comentario comentario = new Comentario();
            comentario.Descripcion = descripcion;
            comentario.VideoID = videoId;
            comentario.UserID = userId;
            comentario.Fecha = DateTime.Now;
            comentario.status = true;
            User entity = new User();
            string foto = "";
            string user = "";
            using (var db = new UvaContext())
            {
                entity = db.Users.Where(x => x.UserID == userId && x.Type == 2).FirstOrDefault();
                foto = db.Users.Where(y => y.UserID == userId).Select(z => z.Foto).FirstOrDefault();
                user = db.Users.Where(x => x.UserID == userId).Select(z=>z.Nombre + z.Apellido).FirstOrDefault();
                if (entity != null)
                    comentario.status = false;
                db.Comentarios.Add(comentario);
                db.SaveChanges();
            }
            string[] result = {foto, user};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComentariosWithId(int videoId)
        {
            UvaContext db = new UvaContext();
            var listadoInicial = db.Comentarios
                .Join(db.Users,
                cm => cm.UserID,
                us => us.UserID,
                (cm, us) => new { Cm = cm, Us = us }).OrderByDescending(y=>y.Cm.Fecha)
                .Where(x => x.Cm.VideoID == videoId).ToList();
            if (listadoInicial != null)
            {

                var result = listadoInicial.Select(y => new { Fecha = y.Cm.Fecha.Day + " / " + y.Cm.Fecha.Month + " / " + y.Cm.Fecha.Year, User = y.Us.Nombre + " " +y.Us.Apellido, Thumb = (y.Us.Foto != null) ? y.Us.Foto : "/videosunefa/img/user-placeholder.jpg", Descripcion = y.Cm.Descripcion, Status = y.Cm.status });
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            return Json(listadoInicial, JsonRequestBehavior.AllowGet);
        }
    }
}