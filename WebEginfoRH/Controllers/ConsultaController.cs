using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        [HttpGet]
        public HttpResponseMessage DownLoadFile(string FileName)
        {

            if (FileName != null)
            {
                var downloadPath = Server.MapPath("~/Upload");
                string filePath = Path.Combine(downloadPath, Path.GetFileName(FileName));
                //string caminhoArquivo = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), FileName));
                /*   FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                   BinaryReader br = new BinaryReader(fs);
                   bytes = br.ReadBytes((Int32)fs.Length);
                   br.Close();
                   fs.Close();*/
                /*HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                System.IO.MemoryStream stream = new MemoryStream(bytes);
                result.Content = new StreamContent(stream);
                result.Content.Headers.Add("x-filename", FileName);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = FileName
                };
                return (result);*/
                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);

                        HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                        httpResponseMessage.Content = new ByteArrayContent(bytes.ToArray());
                        httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        httpResponseMessage.Content.Headers.ContentDisposition.FileName = FileName;
                        httpResponseMessage.Content.Headers.Add("x-filename", FileName);
                        httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");                        
                        httpResponseMessage.StatusCode = HttpStatusCode.OK;
                        return httpResponseMessage;
                    }
                }
            }
            return null;
        }

        [HttpGet]
        public FileResult Download(string FileName)
        {
            var downloadPath = Server.MapPath("~/Upload");
            string filePath = Path.Combine(downloadPath, Path.GetFileName(FileName));

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }


        [HttpGet]
        public HttpResponseMessage DownLoadFile1(string FileName)
        {
            Byte[] bytes = null;
            if (FileName != null)
            {
                var downloadPath = Server.MapPath("~/Upload");
                string filePath = Path.Combine(downloadPath, Path.GetFileName(FileName));
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                bytes = br.ReadBytes((Int32)fs.Length);
                br.Close();
                fs.Close();
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            System.IO.MemoryStream stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = FileName
            };
            return (result);
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
                 CaminhoCurriculo = uploadPath + c.curriculo,
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
