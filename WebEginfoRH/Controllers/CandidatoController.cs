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
    public class CandidatoController : Controller
    {
        private EGINFORHContext db = new EGINFORHContext();

        // GET: Candidato
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public JsonResult GetEspecialidade()
        {
            EGINFORHContext db = new EGINFORHContext();
            var lista = db.Especialidades.Select(n => n).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidato(int[] esp)
        {
            EGINFORHContext db = new EGINFORHContext();
            var result = (
             from a in db.Especialidades
             from b in a.Candidatos
             join c in db.Candidatos on b.id equals c.id
             where esp.Contains(a.id)
             select new DTOGenericObject
             {
                 ID = c.id,
                 Nome = c.nome,
                // Lista = c.Especialidades
             }
             );

          
            /* var accountNumbers = db.Especialidades.Select(x => x.Candidatos).ToArray();

             var duplicationChecklist =
                     from profile in db.Especialidades
                                                  .Where(p => accountNumbers
                                                                 .Contains(esp))
                                                  .AsEnumerable() // Continue in memory
         join param in lstFarmerProfiles on
                         new { profile.Name, profile.AccountNo } equals
                         new { param.Name, param.AccountNo }
                     select profile
            // return result;*/


            /* int[] espec = { 1, 2, 3 };
             var xpto = (from d in db.Candidatos
                         join e in db.Enderecos on d.idEndereco equals e.idEndereco
                         where d.idEndereco == esp
                         select d).ToList();*/



            /*   var lista = (from x in xpto.AsEnumerable()
                            join e in db.Especialidades
                               .Select(x => x.Candidatos).ToArray();
                               */

            // var users = db.Especialidades.Where(r => xpto.AsEnumerable().Any(s => s == r.Candidatos)).SelectMany(r => r.nome);
            // Candidato o = db.Candidatos.Include("tb_Detalhe_Candidato").FirstOrDefault();
            // var result = (from od in db.Especialidades select od).ToList().Select(x => new { id = o.id });


            /* var users = db.Especialidades.Where(r => db.Candidatos.Any(s => s == r.Candidatos)).SelectMany(r => r.nome);

             var especialidades = db.Especialidades.Select(s => s.id).ToList();*/

            //var lista = db.Candidatos.Select(n => n).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public int Salvar(Candidato candidato, ICollection<int> especialidade, Endereco endereco)
        {


            db.Enderecos.Add(endereco);
            db.SaveChanges();
            int idEndereco = endereco.idEndereco;


            var xpto = (from d in db.Especialidades.ToList()
                        join e in especialidade on d.id equals e
                        select d).ToList();

            candidato.idEndereco = idEndereco;
            candidato.Especialidades = xpto;
            db.Candidatos.Add(candidato);
            db.SaveChanges();

            return candidato.id;

        }
    }
}