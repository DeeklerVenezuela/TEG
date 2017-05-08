using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.IO;
using NReco.VideoConverter;
using System.Web.Http.Cors;
using System.Drawing;
using System.Drawing.Imaging;
using UvaWeb.Models;
using System.Text.RegularExpressions;

namespace UvaWeb.Controllers.Videos
{
    public class UploadVideoController : ApiController
    {
        // UploadVideo
        //debe recibir un FormData con los archivos seleccionados, un string nombre y un string descripcion
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult VideoUpload( )
        {
            var request = HttpContext.Current.Request;
            var nombre = HttpContext.Current.Request.Form["nombre"].ToString();
            var descripcion = HttpContext.Current.Request.Form["descripcion"].ToString();
            var userId = HttpContext.Current.Request.Form["userId"];
            var status = HttpContext.Current.Request.Form["status"];
            if (request.Files.Count > 0)
            {
                
                foreach(string file in request.Files)
                {
                    var postedFile = request.Files[file];
                    string pic = System.IO.Path.GetFileName(postedFile.FileName);
                    string ext = Path.GetExtension(pic);
                    string rn = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string userPath = userId;
                    string path = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/Img"), pic);
                    string l = "D:";
                    var code = generateCode();
                    string completePath = l + "//tegjeanrobles//videosunefa//" + userPath + "//video//" + code + "//";
                    createDirectory(completePath);//Crear carpeta para el usuario
                    string path2 = completePath + pic;
                    postedFile.SaveAs(path2);
                    var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
                    ConvertSettings cs = new ConvertSettings();
                    ConvertSettings cs2 = new ConvertSettings();
                    cs.SetVideoFrameSize(640,480);
                    cs2.SetVideoFrameSize(192, 144); 
                    ffmpeg.ConvertMedia(completePath + pic, ext.Replace(".",""), completePath + rn + "_xl.mp4", Format.mp4,cs);
                    ffmpeg.ConvertMedia(completePath + pic, ext.Replace(".",""), completePath + rn + "_sm.mp4", Format.mp4, cs2);

                    createDirectory(completePath + "//thumb");
                    ffmpeg.GetVideoThumbnail(completePath + rn + "_xl.mp4", completePath +"//thumb//" + rn + ".jpeg", 5);

                    //Resize image
                    System.Drawing.Image Image = System.Drawing.Image.FromFile(completePath + "//thumb//" + rn + ".jpeg");
                    
    
                    Bitmap bm = new Bitmap(Image,80,80);
                    //bm.Save(completePath + "//thumb//" + rn + "_sm.jpeg");
                    ApplyCompressionAndSave(bm, completePath + "//thumb//" + rn + "_sm.jpeg", 90, "image/jpeg");

                    System.IO.File.Delete(completePath + pic);


                    var ruta =  "/videosunefa/" + userPath + "/video/" + code + "/" + rn;
                    var thumb = "/videosunefa/" + userPath + "/video/" + code + "/thumb/" + rn + "_sm.jpeg";
                    var _Status = Convert.ToBoolean(status);
                    var _User = Convert.ToInt32(userId);
                    registrarVideo(code, nombre, descripcion, _User, _Status, ruta, thumb);//registrar video
                    
                }
                
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }

        public IHttpActionResult FotoUpload()
        {
            var request = HttpContext.Current.Request;
            var userId = HttpContext.Current.Request.Form["id"].ToString();
            if (request.Files.Count > 0)
            {

                foreach (string file in request.Files)
                {
                    var postedFile = request.Files[file];
                    string pic = System.IO.Path.GetFileName(postedFile.FileName);
                    string rn = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string userPath = userId;
                    string path = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/Img"), pic);
                    string l = "D:";
                    string completePath = l + "//tegjeanrobles//videosunefa//" + userPath + "//acount//";
                    createDirectory(completePath);//Crear carpeta para el usuario
                    string path2 = completePath + pic;
                    postedFile.SaveAs(path2);

                    using (System.Drawing.Image Image = System.Drawing.Image.FromFile(path2))
                    {
                        //Resize image
                        Bitmap bm = new Bitmap(Image, 200, 200);
                        //bm.Save(completePath + "//thumb//" + rn + "_sm.jpeg");
                        if (System.IO.File.Exists(completePath + "photo.jpeg"))
                        {
                            System.IO.File.Delete(completePath + "photo.jpeg");
                        }
                        ApplyCompressionAndSave(bm, completePath + "photo.jpeg", 90, "image/jpeg");
                    }

                    System.IO.File.Delete(path2);
                    using (var db = new UvaContext()) {
                        var id = int.Parse(userId);
                        var entity = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                        entity.Foto = "/videosunefa/"+id+"/acount/photo.jpeg";
                        db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }

        private void ApplyCompressionAndSave(Bitmap imagen, string file, long compressionValue, string mimeType)
        {
            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, compressionValue);
            ImageCodecInfo codec = GetEncoderInfo(mimeType);
            if (codec != null)
            
            imagen.Save(file, codec, ep);
            else
            
        throw new Exception("Codec information not found for the mime type specified. Check your values and try again");
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; ++i)
                if (encoders[i].MimeType.ToLower() == mimeType.ToLower())
                    return encoders[i];
            return null;
        }

        private void createDirectory(string path){
            if (!Directory.Exists(path))
            {
                 Directory.CreateDirectory(path);
            }
        }
        public static string registrarVideo(string codigo, string nombre, string descripcion, int userId, bool status, string ruta, string thumb)
        {
            Models.Video entity = new Models.Video();
            using (var db = new UvaWeb.Models.UvaContext())
            {
                entity.Codigo = codigo;
                entity.Fecha = DateTime.Now;
                entity.UserID = userId;
                entity.Status = status;
                entity.Nombre = nombre;
                entity.Descripcion = descripcion;
                entity.URL = ruta;
                entity.Thumb = thumb;
                db.Videos.Add(entity);
                db.SaveChanges();
            }
            return entity.Codigo;
        }

        public static string generateCode()
        {
            Random rnd = new Random();
            int nmr = rnd.Next(10000000, 99999999);
            var result = nmr.ToString();
            using (var db = new Models.UvaContext())
            {
                var code = db.Videos.Where(x => x.Codigo == result).FirstOrDefault();
                if (code != null)
                    generateCode();
            }
            return result;
        }
    }
}