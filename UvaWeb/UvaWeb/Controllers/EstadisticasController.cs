using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class EstadisticasController : Controller
    {

        #region main
        public JsonResult CountEstadisticasProfesor(int id)
        {
            int videos = CountVideo(id);
            int suscripciones = CountSuscripcion(id);
            int reproducciones = CountReproduccionesUlt30(id);
            List<int> result = new List<int>();
            result.Add(videos);
            result.Add(suscripciones);
            result.Add(reproducciones);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EstadisticaGralProfesor(int id)
        {
            int videos = CountVideo(id);
            int suscripciones = CountSuscripcion(id);
            int reproducciones = CountReproducciones(id);
            int likes = CountLikes(id);
            int dislikes = CountDislikes(id);
            int mensajes = CountMensajes(id);
            int sesiones = CountSesiones(id);
            int grupos = CountGrupos(id);
            List<int> result = new List<int>();
            result.Add(videos);
            result.Add(suscripciones);
            result.Add(reproducciones);
            result.Add(likes);
            result.Add(dislikes);
            result.Add(mensajes);
            result.Add(sesiones);
            result.Add(grupos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EstadisticaGralProfesorUltMes(int id)
        {
            var result = EstGralProfesorUltMes(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GraficoGralProfesorUltMes(int id)
        {
            var dataMain = EstGralProfesorUltMes(id);
            Grafico1 g1 = new Grafico1();
            
            List<SeriesMain> serie = new List<SeriesMain>();
            foreach (var i in dataMain) {
                SeriesMain item = new SeriesMain();
                item.name = i.Nombre;
                item.y = i.Valor;
                item.drilldown = i.Nombre;
                serie.Add(item);
            }
            g1.SeriesMes = serie;
            g1.Suscripciones = SuscripcionesTresMeses(id);
            g1.Reproducciones = ReproducionesTresMeses(id);
            g1.Likes = LikesTresMeses(id);
            g1.Dislikes = DisLikesTresMeses(id);
            g1.Mensajes = MensajesTresMeses(id);
            g1.Sesiones = SesionesTresMeses(id);
            return Json(g1, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<DatoEstadistico> EstGralProfesorUltMes(int id)
        {
            List<DatoEstadistico> result = new List<DatoEstadistico>();
            //Ultimos 30
            double suscripciones30 = CountSuscripcionesUlt30(id);
            double reproducciones30 = CountReproduccionesUlt30(id);
            double likes30 = CountLikesUlt30(id);
            double dislikes30 = CountDislikesUlt30(id);
            double mensajes30 = CountMensajesUlt30(id);
            double sesiones30 = CountSesionesUlt30(id);

            //Total
            double suscripciones = CountSuscripcion(id);
            double reproducciones = CountReproducciones(id);
            double likes = CountLikes(id);
            double dislikes = CountDislikes(id);
            double mensajes = CountMensajes(id);
            double sesiones = CountSesiones(id);

           
            //Calculo de promedios
            double promedioSus = (suscripciones > 0) ? suscripciones30 / suscripciones * 100 : 0;
            double promedioRep = (reproducciones > 0) ? reproducciones30 / reproducciones * 100 : 0;
            double promedioLikes = (likes > 0) ? likes30 / likes * 100 : 0;
            double promedioDisL = (dislikes > 0) ? dislikes30 / dislikes * 100 : 0;
            double promedioMen = (mensajes > 0) ? mensajes30 / mensajes * 100 : 0;
            double promedioSes =(sesiones > 0) ? sesiones30 / sesiones * 100 : 0;

            //Preparar Resultado
            DatoEstadistico de = new DatoEstadistico();
            DatoEstadistico de2 = new DatoEstadistico();
            DatoEstadistico de3 = new DatoEstadistico();
            DatoEstadistico de4 = new DatoEstadistico();
            DatoEstadistico de5 = new DatoEstadistico();
            DatoEstadistico de6 = new DatoEstadistico();
            de.Nombre = "Suscripciones";
            de.Valor = Math.Round(promedioSus,2);
            result.Add(de);
            de2.Nombre = "Reproducciones";
            de2.Valor = Math.Round(promedioRep,2);
            result.Add(de2);
            de3.Nombre = "Likes";
            de3.Valor = Math.Round(promedioLikes,2);
            result.Add(de3);
            de4.Nombre = "Dislikes";
            de4.Valor = Math.Round(promedioDisL,2);
            result.Add(de4);
            de5.Nombre = "Mensajes";
            de5.Valor = Math.Round(promedioMen,2);
            result.Add(de5);
            de6.Nombre = "Sesiones";
            de6.Valor = Math.Round(promedioSes,2);
            result.Add(de6);

            return result;
        }

        public JsonResult EstadisticasVideosPorUsuario(int id) {
            
            List<VideoEstadistico> ListaVideos = new List<VideoEstadistico>();
            using (var db = new UvaContext())
            {
                var videos = db.Videos.Where(x => x.UserID == id).ToList<Models.Video>();
                foreach (var i in videos)
                {
                    VideoEstadistico ve = new VideoEstadistico();
                    ve.Likes = db.Likes.Where(x => x.VideoID == i.VideoID && x.Tipo == true).Count();
                    ve.Dislikes = db.Likes.Where(x => x.VideoID == i.VideoID && x.Tipo == false).Count();
                    ve.Suscripciones = db.Suscripcions.Where(x => x.VideoID == i.VideoID).Count();
                    ve.Reproducciones = db.Reproduccions.Where(x => x.VideoID == i.VideoID).Count();
                    ve.Thumb = i.Thumb;
                    ve.Fecha = i.Fecha.Day + " / " + i.Fecha.Month + " / " + i.Fecha.Year;
                    ve.Nombre = i.Nombre;
                    ve.Descripcion = i.Descripcion;
                    ListaVideos.Add(ve);
                }
                return Json(ListaVideos, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region tresmeses
        private Serie SuscripcionesTresMeses(int id)
        {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            var treeMonth = now.AddMonths(-3);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {
                uno = db.Suscripcions
                    .Join(db.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus, vid) => new { Sus = sus, Vid = vid }).Where(y => y.Vid.UserID == id && y.Sus.Fecha.Month == now.Month).Count();
                dos = db.Suscripcions
                    .Join(db.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus, vid) => new { Sus = sus, Vid = vid }).Where(y => y.Vid.UserID == id && y.Sus.Fecha.Month == oneMonth.Month).Count();
                tres = db.Suscripcions
                    .Join(db.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus, vid) => new { Sus = sus, Vid = vid }).Where(y => y.Vid.UserID == id && y.Sus.Fecha.Month == twoMonth.Month).Count();
            }
            Serie suscripciones = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            suscripciones.id = "Suscripciones";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            suscripciones.data = valMes;
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            return suscripciones;
        }

        private Serie ReproducionesTresMeses(int id)
        {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {
                uno = db.Reproduccions
                    .Join(db.Videos,
                    rep => rep.VideoID,
                    vid => vid.VideoID,
                    (rep, vid) => new { Rep = rep, Vid = vid }).Where(x => x.Vid.UserID == id && x.Rep.Fecha.Month == now.Month).Count();
                dos = db.Reproduccions
                    .Join(db.Videos,
                    rep => rep.VideoID,
                    vid => vid.VideoID,
                    (rep, vid) => new { Rep = rep, Vid = vid }).Where(x => x.Vid.UserID == id && x.Rep.Fecha.Month == oneMonth.Month).Count();
                tres = db.Reproduccions
                    .Join(db.Videos,
                    rep => rep.VideoID,
                    vid => vid.VideoID,
                    (rep, vid) => new { Rep = rep, Vid = vid }).Where(x => x.Vid.UserID == id && x.Rep.Fecha.Month == twoMonth.Month).Count();
            }
            Serie reproducciones = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            reproducciones.id = "Reproducciones";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            reproducciones.data = valMes;
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            return reproducciones;
        }

        private Serie LikesTresMeses(int id)
        {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {
                uno = db.Likes
                    .Join(db.Videos,
                    li => li.VideoID,
                    vi => vi.VideoID,
                    (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == true && x.Li.Fecha.Month == now.Month).Count();
                dos = db.Likes
                     .Join(db.Videos,
                     li => li.VideoID,
                     vi => vi.VideoID,
                     (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == true && x.Li.Fecha.Month == oneMonth.Month).Count();
                tres = db.Likes
                     .Join(db.Videos,
                     li => li.VideoID,
                     vi => vi.VideoID,
                     (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == true && x.Li.Fecha.Month == twoMonth.Month).Count();
                 
      
            }
            Serie likes = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            likes.id = "Likes";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            likes.data = valMes;
            
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            return likes;
        }

        private Serie DisLikesTresMeses(int id) {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {
                uno = db.Likes
                    .Join(db.Videos,
                    li => li.VideoID,
                    vi => vi.VideoID,
                    (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == false && x.Li.Fecha.Month == now.Month).Count();
                dos = db.Likes
                     .Join(db.Videos,
                     li => li.VideoID,
                     vi => vi.VideoID,
                     (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == false && x.Li.Fecha.Month == oneMonth.Month).Count();
                tres = db.Likes
                     .Join(db.Videos,
                     li => li.VideoID,
                     vi => vi.VideoID,
                     (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == false && x.Li.Fecha.Month == twoMonth.Month).Count();


            }
            Serie dislikes = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            dislikes.id = "Dislikes";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            dislikes.data = valMes;
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            
            return dislikes;
        }

        private Serie MensajesTresMeses(int id)
        {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {

                uno = db.Mensajes
                    .Join(db.Videos,
                    mn => mn.VideoID,
                    vi => vi.VideoID,
                    (mn, vi) => new { Mn = mn, Vi = vi }).Where(x => x.Vi.UserID == id && x.Mn.Fecha.Month == now.Month).Count();
                dos = db.Mensajes
                       .Join(db.Videos,
                       mn => mn.VideoID,
                       vi => vi.VideoID,
                       (mn, vi) => new { Mn = mn, Vi = vi }).Where(x => x.Vi.UserID == id && x.Mn.Fecha.Month == oneMonth.Month).Count();
                tres = db.Mensajes
                           .Join(db.Videos,
                           mn => mn.VideoID,
                           vi => vi.VideoID,
                           (mn, vi) => new { Mn = mn, Vi = vi }).Where(x => x.Vi.UserID == id && x.Mn.Fecha.Month == twoMonth.Month).Count();
            }
            Serie mensajes = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            mensajes.id = "Mensajes";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            mensajes.data = valMes;
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            return mensajes;
        }

        private Serie SesionesTresMeses(int id)
        {
            var now = DateTime.Now;
            var oneMonth = now.AddMonths(-1);
            var twoMonth = now.AddMonths(-2);
            int uno = 0;
            int dos = 0;
            int tres = 0;
            using (var db = new UvaContext())
            {
                uno = db.Sessions.Where(x => x.UserID == id && x.Fecha.Month == now.Month).Count();
                dos = db.Sessions.Where(x => x.UserID == id && x.Fecha.Month == oneMonth.Month).Count();
                tres = db.Sessions.Where(x => x.UserID == id && x.Fecha.Month == twoMonth.Month).Count();
            }
            Serie sesiones = new Serie();
            List<ValorMes> valMes = new List<ValorMes>();
            sesiones.id = "Sesiones";
            //Menos 3 meses
            ValorMes v3 = new ValorMes();
            v3.Mes = buscarMes(2);
            v3.Valor = tres;
            valMes.Add(v3);
            sesiones.data = valMes;
            
            //Menos 2 meses
            ValorMes v2 = new ValorMes();
            v2.Mes = buscarMes(1);
            v2.Valor = dos;
            valMes.Add(v2);
            //Menos 1 mes
            ValorMes v1 = new ValorMes();
            v1.Mes = buscarMes(0);
            v1.Valor = uno;
            valMes.Add(v1);
            return sesiones;
        }

        #endregion

        #region generic
        private string buscarMes(int mes){
            var mesCalcular = DateTime.Now.AddMonths(-mes);
            var result = mesCalcular.ToString("MMM");
            return result;
        }

        #endregion

        #region general

        private int CountVideo(int id)
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Videos.Where(x => x.UserID == id).Count();
            }
            return result;
        }

        private int CountSuscripcion(int id)
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Suscripcions
                    .Join(db.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus, vid) => new { Sus = sus, Vid = vid }).Where(y => y.Vid.UserID == id).Count();

            }
            return result;
        }

        private int CountReproducciones(int id)
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Reproduccions
                    .Join(db.Videos,
                    rep => rep.VideoID,
                    vid => vid.VideoID,
                    (rep, vid) => new { Rep = rep, Vid = vid }).Where(x => x.Vid.UserID == id).Count();
            }
            return result;
        }

        public int CountLikes(int id)
        {
            int result = 0;
            using (var db = new UvaContext()) {
                    result = db.Likes
                    .Join(db.Videos,
                    li => li.VideoID,
                    vi => vi.VideoID,
                    (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == true).Count();
            }
            return result;
        }

        private int CountDislikes(int id)
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Likes
                .Join(db.Videos,
                li => li.VideoID,
                vi => vi.VideoID,
                (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == false).Count();
            }
            return result;
        }

        private int CountMensajes(int id) 
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Mensajes
                .Join(db.Videos,
                mn => mn.VideoID,
                vi => vi.VideoID,
                (mn, vi) => new { Mn = mn, Vi = vi }).Where(x => x.Vi.UserID == id).Count();
            }
            return result;
        }

        private int CountSesiones(int id) 
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Sessions.Where(x => x.UserID == id).Count();
            }
            return result;
        }

        private int CountGrupos(int id)
        {
            int result = 0;
            using (var db = new UvaContext())
            {
                result = db.Unions.Where(x => x.UserID == id).Count();
            }
            return result;
        }
        #endregion

        #region 30dias
        private int CountReproduccionesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Reproduccions
                    .Join(db.Videos,
                    rep => rep.VideoID,
                    vid => vid.VideoID,
                    (rep, vid) => new { Rep = rep, Vid = vid }).Where(x => x.Vid.UserID == id && x.Rep.Fecha > oneMonth && x.Rep.Fecha <= now).Count();
            }
            return result;
        }

        private int CountLikesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Likes
                .Join(db.Videos,
                li => li.VideoID,
                vi => vi.VideoID,
                (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == true && x.Li.Fecha > oneMonth && x.Li.Fecha <= now).Count();
            }
            return result;
        }

        private int CountDislikesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Likes
                .Join(db.Videos,
                li => li.VideoID,
                vi => vi.VideoID,
                (li, vi) => new { Li = li, Vi = vi }).Where(x => x.Vi.UserID == id && x.Li.Tipo == false && x.Li.Fecha > oneMonth && x.Li.Fecha <= now).Count();
            }
            return result;

        }

        private int CountSesionesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Sessions.Where(x => x.UserID == id && x.Fecha > oneMonth && x.Fecha <= now).Count();
            }
            return result;
        }

        private int CountMensajesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Mensajes
                .Join(db.Videos,
                mn => mn.VideoID,
                vi => vi.VideoID,
                (mn, vi) => new { Mn = mn, Vi = vi }).Where(x => x.Vi.UserID == id && x.Mn.Fecha > oneMonth && x.Mn.Fecha <= now).Count();
            }
            return result;   
        }

        private int CountSuscripcionesUlt30(int id)
        {
            int result = 0;
            var now = DateTime.Now;
            var oneMonth = now.AddDays(-30);
            using (var db = new UvaContext())
            {
                result = db.Suscripcions
                    .Join(db.Videos,
                    sus => sus.VideoID,
                    vid => vid.VideoID,
                    (sus, vid) => new { Sus = sus, Vid = vid }).Where(y => y.Vid.UserID == id && y.Sus.Fecha > oneMonth && y.Sus.Fecha <= now).Count();
            }
            return result;
        }
        #endregion



    }
}