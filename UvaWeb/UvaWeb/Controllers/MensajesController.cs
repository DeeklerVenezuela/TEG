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
    public class MensajesController : Controller
    {
        private UvaContext db = new UvaContext();//Contexto de la base de datos
        public ActionResult Index()
        {
            return View();
        }

        //Buscar todos los mensajes------------------------------------------------------------------------------

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarMensajes(int id)
        {
            var user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
            if (user.Type == 2)
            {
                var Chats = db.Mensajes.Join(db.Videos,
                  msj => msj.VideoID,
                  vid => vid.VideoID,
                  (msj, vid) => new { Mj = msj, Vd = vid })
                  .Join(db.Users,
                  vd => vd.Mj.UserID,
                  usr => usr.UserID,
                  (vd, usr) => new { vd, usr })
                  .Join(db.Chats,
                  mj => mj.vd.Mj.Chat,
                  ch => ch.ChatID,
                  (mj, ch) => new { Mj = mj, Ch = ch })
                  .Where(chats => chats.Mj.vd.Mj.UserID == id || chats.Mj.vd.Vd.UserID == id).ToList();//Join 3 tablas usuario, mensaje, video

                var groups = Chats.GroupBy(x => x.Ch.ChatID);//Agrupar por video id

                var arrays = groups.Select(x => x.Select(y => new { VideoID = y.Mj.vd.Vd.VideoID, id = y.Mj.vd.Mj.MensajeID, VideoNombre = y.Mj.vd.Vd.Nombre, VideoDescripcion = y.Mj.vd.Vd.Descripcion, NombreProfesor = y.Mj.usr.Nombre, ApellidoProfesor = y.Mj.usr.Apellido, fecha = y.Mj.vd.Vd.Fecha.Day + "/" + y.Mj.vd.Vd.Fecha.Month + "/" + y.Mj.vd.Vd.Fecha.Year, status = y.Mj.vd.Mj.status, thumb = y.Mj.usr.Foto, chat = y.Ch.ChatID }).ToList()).ToList();//Seleccionar repuesta

                return Json(arrays, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON
            } 
            if (user.Type == 1)
            {
                var Chats = db.Mensajes.Join(db.Videos,
                    msj => msj.VideoID,
                    vid => vid.VideoID,
                    (msj, vid) => new { Mj = msj, Vd = vid })
                    .Join(db.Users,
                    vd => vd.Vd.UserID,
                    usr => usr.UserID,
                    (vd, usr) => new { vd, usr })
                    .Join(db.Chats,
                    mj => mj.vd.Mj.Chat,
                    ch => ch.ChatID,
                    (mj, ch) => new { Mj = mj, Ch = ch })
                    .Where(chats => chats.Mj.vd.Mj.UserID == id || chats.Mj.vd.Vd.UserID == id).ToList();//Join 3 tablas usuario, mensaje, video

                var groups = Chats.GroupBy(x => x.Ch.ChatID);//Agrupar por video id

                var arrays = groups.Select(x => x.Select(y => new { VideoID = y.Mj.vd.Vd.VideoID, id = y.Mj.vd.Mj.MensajeID, VideoNombre = y.Mj.vd.Vd.Nombre, VideoDescripcion = y.Mj.vd.Vd.Descripcion, NombreProfesor = y.Mj.usr.Nombre, ApellidoProfesor = y.Mj.usr.Apellido, fecha = y.Mj.vd.Vd.Fecha.Day + "/" + y.Mj.vd.Vd.Fecha.Month + "/" + y.Mj.vd.Vd.Fecha.Year, status = y.Mj.vd.Mj.status, thumb = y.Mj.usr.Foto, chat = y.Ch.ChatID }).ToList()).ToList();//Seleccionar repuesta

                return Json(arrays, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON
            }
            string[] bad = { "Error" };
            return Json(bad, JsonRequestBehavior.AllowGet);
            
        }


        //Buscar Mensajes por video ----------------------------------------------------------------------------------
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarMensajesPorVideo(int user, int chat)
        {
            var Chats = db.Mensajes
                .Join(db.Chats,
                ms => ms.Chat,
                ch => ch.ChatID,
                (ms, ch) => new { Ms =ms , Ch = ch})
                .Where(x => x.Ms.Chat == chat)
                .Select(y => new { Descripcion = y.Ms.Descripcion, Fecha = y.Ms.Fecha.Day + "/" + y.Ms.Fecha.Month + "/" + y.Ms.Fecha.Year, Status = (y.Ms.UserID == user) ? true : false }).ToList();
            return Json(Chats, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult BuscarFotoProfesorChat(int id)
        {
          

            var Chats = db.Videos.Join(db.Users,
                vid => vid.UserID,
                usr => usr.UserID,
                (vid, usr) => new { VD = vid, US = usr }).Where(x => x.VD.VideoID == id)
                .Select(y => new { Foto = y.US.Foto, Nombre = y.US.Nombre + " " +y.US.Apellido}).FirstOrDefault();//Buscamos Foto del profesor

            
            return Json(Chats, JsonRequestBehavior.AllowGet);//Retornar objeto JSON
        }



        //Insertar un mensaje ---------------------------------------------------------------------------------------------
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult InsertarMensaje(int id, string body, int videoID, string chat )
        {
            using (var dbc = new UvaContext())
            {
                Mensaje msj = new Mensaje();
                Chat chatSearch = new Chat();
                Chat chatNuevo = new Chat();
                bool createChat = false;
                chatNuevo = dbc.Chats.Where(x => x.UserID == id && x.VideoID == videoID).FirstOrDefault();
                if (String.IsNullOrEmpty(chat) && chatNuevo == null)
                {
                    Chat nuevoChat = new Chat();
                    nuevoChat.UserID = id;
                    nuevoChat.VideoID = videoID;
                    dbc.Chats.Add(nuevoChat);
                    dbc.SaveChanges();
                    createChat = true;
                }
                if (!String.IsNullOrEmpty(chat))
                {
                    var idNuevo = int.Parse(chat);
                    chatSearch = dbc.Chats.Where(x => x.ChatID == idNuevo).FirstOrDefault();
                }
                if (createChat == true)
                {
                    chatSearch = dbc.Chats.Where(x => x.UserID == id && x.VideoID == videoID).FirstOrDefault();
                }
                var Video = dbc.Videos.Where(x => x.VideoID == videoID).FirstOrDefault();
                if (chatSearch == null && Video.UserID == id)
                {
                    string[] bad = { "Disculpe", "Los docentes no pueden iniciar los chats, solo pueden responder mensajes" };
                    return Json(bad, JsonRequestBehavior.AllowGet);
                }
                if (chatSearch != null)
                    msj.Chat = chatSearch.ChatID;
                if(chatNuevo != null)
                    msj.Chat = chatNuevo.ChatID;
                msj.VideoID = videoID;
                msj.Fecha = DateTime.Now;
                msj.status = true;
                msj.UserID = id;
                msj.Descripcion = body;
                dbc.Mensajes.Add(msj);
                dbc.SaveChanges();
            }
            string[] respuesta = {"Felicidades!!!","Mensaje enviado con exito"};
            return Json(respuesta, JsonRequestBehavior.AllowGet);//Retornar string JSON
        }

        public JsonResult DeleteConversacion(int id)
        {
            Chat chat = new Chat();
            List<Mensaje> mensajes = new List<Mensaje>();
            using (var db = new UvaContext())
            {
                chat = db.Chats.Where(x => x.ChatID == id).FirstOrDefault();
                mensajes = db.Mensajes.Where(x => x.Chat == id).ToList();
                if (mensajes != null)
                {
                    foreach (var i in mensajes)
                    {
                        db.Mensajes.Remove(i);
                    }
                }
                if (chat != null)
                {
                    db.Chats.Remove(chat);
                }
                db.SaveChanges();
            }
            string[] result = { "Felicidades", "Conversación eliminada exitosamente."};
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}