using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEginfoRH.Models;

namespace WebEginfoRH.Controllers
{
    public class CadastroController : Controller
    {
        private EGINFORHContext db = new EGINFORHContext();

        // GET: Cadastro
        public ActionResult Index()
        {
            return View(db.Candidatos.ToList());
        }

        public JsonResult GetEspecialidade()
        {
            EGINFORHContext db = new EGINFORHContext();
            var lista =  db.Especialidades.Select(n => n).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPerfil()
        {
            List<SelectListItem> perfis = new List<SelectListItem>();
            {
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Lider", Value = "1" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Senior", Value = "2" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Pleno", Value = "3" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Junior", Value = "4" });
            };

            //  var ret = db.LocationTbls.Select(x => new { x.Id, x.LocName }).ToList();

            return Json(perfis, JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> Get()
        {
            List<SelectListItem> perfis = new List<SelectListItem>();
            {
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Lider", Value = "1" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Senior", Value = "2" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Pleno", Value = "3" });
                perfis.Add(new System.Web.Mvc.SelectListItem { Text = "Junior", Value = "4" });
            };
            return perfis;
        }
        [HttpPost]
        public void Upload(System.Web.HttpPostedFileBase aFile)
        {
            string file = aFile.FileName;
            string path = Server.MapPath("../Upload//");
            aFile.SaveAs(path + Guid.NewGuid() + "." + file.Split('.')[1]);
        }
        [HttpPost]
        public int Salvar(Candidato candidato, ICollection<Especialidade> especialidade)
        {
            candidato.Especialidades = especialidade.ToList();
            db.Candidatos.Add(candidato);
            db.SaveChanges();

           
            return candidato.id;

        }

       /* [HttpPost]
        public int Salvar_Especialidades(tb_Candidato especialidade)
        {
            /*EGINFO_RHModel.tb_Detalhe_Candidato;
            db..Add(especialidade);
            db.SaveChanges();
            return candidato.id;

        }*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
