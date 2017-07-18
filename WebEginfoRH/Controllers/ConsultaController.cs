using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEginfoRH.Models;

namespace WebEginfoRH.Controllers
{
    public class ConsultaController : Controller
    {
        private EGINFORHContext db = new EGINFORHContext();
        // GET: Consulta
        public ActionResult Index()
        {
            var candidatoes = db.Candidatos.Include(c => c.Endereco);
            return View(candidatoes.ToList());
        }

        public ActionResult Consulta()
        {

            var candidatoes = db.Candidatos.Include(c => c.Endereco);
            return View(candidatoes.ToList());
        }

        public JsonResult Buscar(int idPerfil, IEnumerable<int> especialidade)
        {
            EGINFORHContext db = new EGINFORHContext();
            var uploadPath = Server.MapPath("~/Upload/");
            var result = (
             from a in db.Especialidades
             from b in a.Candidatos
             join c in db.Candidatos.Include("tb_Endereco") on b.id equals c.id
             where especialidade.Contains(a.id)
             select new DTOGenericObject
             {
                 ID = c.id,
                 Nome = c.nome,
                 Celular = c.celular,
                 Email = c.email,
                 CaminhoCurriculo = uploadPath,
                 Curriculo = c.curriculo,
                 EnderecoCidade = c.Endereco.cidade,
                 IdPerfil = c.idPerfil
             }

             ).Distinct();

            if (idPerfil != 0)
            {
                result = result.Where(c => c.IdPerfil == idPerfil);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEspecialidade()
        {
            EGINFORHContext db = new EGINFORHContext();
            var lista = db.Especialidades.Select(n => n).ToList();
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
            return Json(perfis, JsonRequestBehavior.AllowGet);
        }
        /*
        // GET: Consulta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidato candidato = db.Candidatoes.Find(id);
            if (candidato == null)
            {
                return HttpNotFound();
            }
            return View(candidato);
        }

        // GET: Consulta/Create
        public ActionResult Create()
        {
            ViewBag.idEndereco = new SelectList(db.Enderecoes, "idEndereco", "logradouro");
            return View();
        }

        // POST: Consulta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nome,cpf,telefone,celular,email,senha,idPerfil,idEndereco,curriculo")] Candidato candidato)
        {
            if (ModelState.IsValid)
            {
                db.Candidatoes.Add(candidato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEndereco = new SelectList(db.Enderecoes, "idEndereco", "logradouro", candidato.idEndereco);
            return View(candidato);
        }

        // GET: Consulta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidato candidato = db.Candidatoes.Find(id);
            if (candidato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEndereco = new SelectList(db.Enderecoes, "idEndereco", "logradouro", candidato.idEndereco);
            return View(candidato);
        }

        // POST: Consulta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,cpf,telefone,celular,email,senha,idPerfil,idEndereco,curriculo")] Candidato candidato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEndereco = new SelectList(db.Enderecoes, "idEndereco", "logradouro", candidato.idEndereco);
            return View(candidato);
        }

        // GET: Consulta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidato candidato = db.Candidatoes.Find(id);
            if (candidato == null)
            {
                return HttpNotFound();
            }
            return View(candidato);
        }

        // POST: Consulta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Candidato candidato = db.Candidatoes.Find(id);
            db.Candidatoes.Remove(candidato);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
