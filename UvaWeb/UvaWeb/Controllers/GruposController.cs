using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using UvaWeb.Models;

namespace UvaWeb.Controllers
{
    public class GruposController : Controller
    {

        private UvaContext db = new UvaContext();//Contexto de la base de datos


        // GET: Grupos
        public ActionResult Index()
        {
            return View();
        }



        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public virtual JsonResult BuscarProfesoresEnGrupos(int id)
        {
            var profes = BuscarProfesoresG(id);
            return Json(profes, JsonRequestBehavior.AllowGet);//Retornar arreglo JSON
        }

        public static List<Models.User> BuscarProfesoresG(int id){
            UvaContext db = new UvaContext();
            var groups = db.Unions.Where(y => y.UserID == id).Select(x => x.GrupoID).ToList();
            List<Models.User> users = new List<User>();
            if (groups != null)
            {
                foreach (var i in groups)
                {
                    var use = db.Unions.Join(db.Users,
                        gr => gr.UserID,
                        us => us.UserID,
                        (gr, us) => new { gr, us }).Where(usu => usu.us.Type == 2 && usu.gr.GrupoID == i && usu.us.Status != false)
                        .Select(x => x.us).ToList();
                    if (use != null)
                    {
                        foreach (var j in use)
                        {
                            j.Password = "";
                            j.Email = "";
                            j.Type = 0;
                            if (j.Foto == null)
                            {
                                j.Foto = "/videosunefa/img/user-placeholder.jpg";
                            }
                            users.Add(j);
                        }

                    }
                }
            }
            return users;
        }

        public JsonResult GetGroupsById(int id)
        {
            List<SuscripcionGrupo> result = new List<SuscripcionGrupo>();
            result = BuscarProfesores(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public static List<SuscripcionGrupo> BuscarProfesores(int id){
            List<SuscripcionGrupo> resulta = new List<SuscripcionGrupo>();
            using (var dbc = new UvaContext())
            {
                var groups = dbc.Users.Join(dbc.Unions,
                    us => us.UserID,
                    un => un.UserID,
                    (us, un) => new { Un = un.UserID, Ung = un.GrupoID })
                    .Join(dbc.Grupos,
                    uni => uni.Ung,
                    gru => gru.GrupoID,
                    (uni, gru) => new { Uni = uni.Un, Car = gru.Carrera, Sec = gru.Seccion, Sem = gru.Semestre, Tur = gru.Turno, Gi = gru.GrupoID })
                    .Where(x => x.Uni == id).ToList();

                for (int i = 0; i < groups.Count; i++)
                {
                    SuscripcionGrupo sg = new SuscripcionGrupo();
                    sg.Grupo = groups[i].Sem + " | " + groups[i].Car + " | " + groups[i].Sec + " | " + groups[i].Tur;
                    sg.GrupoId = groups[i].Gi;
                    resulta.Add(sg);
                }
            }
            return resulta;
        }

        public JsonResult BuscarCarreras() {
            using (var dbc = new UvaContext())
            {
                var gr = dbc.Grupos.Select(x => x.Carrera).ToList();
                if (gr != null)
                {
                    var grt = gr.Distinct();
                    return Json(grt, JsonRequestBehavior.AllowGet);
                }
            }
            string[] result = {"Nada encontrado"};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarSemestre(string carrera) {
            using (var dbc = new UvaContext())
            {
                var cr = dbc.Grupos.Where(x => x.Carrera == carrera).Select(y=>y.Semestre).ToList();
                if (cr != null)
                {
                    var semestres = cr.Distinct();
                    return Json(semestres, JsonRequestBehavior.AllowGet);
                }
            }
            
            string[] result = {"Nada encontrado"};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarTurno(string carrera, int semestre)
        {
            using (var dbc = new UvaContext()) {
                var gr = dbc.Grupos.Where(x => x.Carrera == carrera && x.Semestre == semestre).Select(y=>y.Turno).ToList();
                if (gr != null)
                {
                    var turnos = gr.Distinct();
                    return Json(turnos, JsonRequestBehavior.AllowGet);
                }
            }
            string[] result = { "Nada encontrado" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarSeccion(string carrera, int semestre, string turno)
        {
            using (var dbc = new UvaContext())
            {
                var gr = dbc.Grupos.Where(x => x.Carrera == carrera && x.Semestre == semestre && x.Turno == turno).Select(y=>y.Seccion).ToList();
                if (gr != null)
                {
                    var secciones = gr.Distinct(); 
                    return Json(secciones, JsonRequestBehavior.AllowGet);
                }
            }
            string[] result = { "Nada encontrado" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarGrupo(string carrera, int semestre, string turno, string seccion, int id)
        {
            string[] result = {"Lo Sentimos","Usted no puede unirse a este grupo"};
            var verificar = verificarGrupo(carrera, semestre, turno, seccion, id);
            var cg = contarGrupos(id);
            if (cg < 3)
            {
                if (verificar == true)
                {
                    var verfCarrera = verificarCarrera(id);
                    if (verfCarrera == carrera)
                    {
                        Union entity = new Union();
                        using (var dbc = new UvaContext())
                        {
                            entity.GrupoID = buscarGrupo(carrera, semestre, turno, seccion);
                            entity.Fecha = DateTime.Now;
                            entity.UserID = id;
                            dbc.Unions.Add(entity);
                            dbc.SaveChanges();
                            string[] great = { "Felicidades", "Usted se ha unido al grupo exitosamente." };
                            return Json(great, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                else
                {
                    string[] err = { "Lo Sentimos", "Usted no puede unirse a este grupo." };
                    return Json(err, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet); 
        }

        private string verificarCarrera(int id)
        {
            Models.CarreraUser carrera = new Models.CarreraUser();
            using (var db = new UvaContext())
            {
                var user = db.Users.Where(x => x.UserID == id).FirstOrDefault();
                carrera = db.CarrerasUsers.Where(x => x.CarreraID == user.CarreraId).FirstOrDefault();   
            }
            if (carrera != null)
                return carrera.NombreCarrera;
            else
                return "error user no encontrado";
        }

        private int contarGrupos(int id)
        {
            int result = 0;
            using (var dbc = new UvaContext())
            {
                result = dbc.Unions.Where(x => x.UserID == id).Count();
            }
            return result;
        }
        private bool verificarGrupo(string carrera, int semestre, string turno, string seccion, int id)
        {
            IEnumerable<Union> UnionsUser = null;
            int? grupoId = buscarGrupo(carrera, semestre, turno, seccion);
            using (var dbc = new UvaContext())
            {
                UnionsUser = dbc.Unions.Where(x => x.UserID == id).ToList();    
            }

            if (UnionsUser == null)
            {
                return true;
            }
                

            if (UnionsUser != null)
            {
                var res = UnionsUser.Where(x => x.GrupoID == grupoId).FirstOrDefault();
                if (res == null)
                {
                    using (var db = new UvaContext())
                    {
                        var t = db.Unions.Where(x => x.UserID == id).ToList();
                        if (t.Count >= 2)
                        {
                            return false;
                        }
                        else
                        {
                            var gs = db.Unions.Join(db.Grupos,
                                un => un.GrupoID,
                                gr => gr.GrupoID,
                                (un, gr) => new { Un = un, Gr = gr })
                                .Where(x => x.Un.UserID == id && x.Gr.Semestre == semestre).FirstOrDefault();

                            if (gs != null)
                                return false;
                            else
                                return true;
                        }

                    }
        
                }
            }
            return false;
        }



        


        private int buscarGrupo(string carrera, int semestre, string turno, string seccion)
        {
            int grupoId = 0;
            using (var dbc = new UvaContext())
            {
                grupoId = dbc.Grupos
                             .Where(x => x.Carrera == carrera && x.Semestre == semestre && x.Seccion == seccion && x.Turno == turno)
                             .Select(y => y.GrupoID)
                             .FirstOrDefault();
            }
            return grupoId;
        }

        public JsonResult GetGroupForDelete(int id){
            List<Grupo> result = new List<Grupo>();
            using (var db = new UvaContext()) {
                var grupo = db.Grupos.Where(x => x.GrupoID == id).FirstOrDefault();
                result.Add(grupo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSuscription(int id, int user)
        {
            string[] result = { "Lo Sentimos", "Usted no puede unirse a este grupo" };
            using (var db = new UvaContext())
            {
                var grupo = db.Grupos.Join(
                    db.Unions,
                    gr => gr.GrupoID,
                    un => un.GrupoID,
                    (gr, un) => new { Gr = gr, Un = un })
                    .Where(y => y.Un.UserID == user && y.Gr.GrupoID == id)
                    .FirstOrDefault();
                var union = grupo.Un;
                if (grupo != null)
                {
                    db.Unions.Remove(union);
                    db.SaveChanges();
                    string[] great = { "Felicidades!!!", "Usted ha cancelado su union al grupo" };
                    return Json(great, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}