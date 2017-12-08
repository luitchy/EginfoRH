using Apitron.PDF.Kit;
using Apitron.PDF.Kit.Extraction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using WebEginfoRH.Models;

namespace WebApiEginfo.Controllers
{
    public class CandidatoController : Controller
    {

        private EGINFORHContext db = new EGINFORHContext();
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }

        public JsonResult Buscar(int idPerfil, IEnumerable<int> especialidade, string cidade)
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


            if (!string.IsNullOrEmpty(cidade))
            {
                result = result.Where(c => c.EnderecoCidade.Contains(cidade));
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Download(string FileName)
        {
            var downloadPath = Server.MapPath("~/Upload");
            string filePath = Path.Combine(downloadPath, Path.GetFileName(FileName));

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            string extensao = Path.GetExtension(filePath);
            string contentType = string.Empty;
            if (extensao.Equals(".pdf"))
                contentType =  "application/pdf";

            if (extensao.Equals(".docx"))
                contentType = "application/docx";

            Response.AppendHeader("Content-Disposition", contentType.ToString());
            return File(fileBytes, contentType);


        }

        [HttpGet]
        public FileStreamResult Download1(string FileName)
        {
            var downloadPath = Server.MapPath("~/Upload");
            string filePath = Path.Combine(downloadPath, Path.GetFileName(FileName));
            string name = Server.MapPath("~/out.html");
            FileInfo info = new FileInfo(name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            Stream stream = new MemoryStream(fileBytes);
            string extensao = Path.GetExtension(filePath);
            string contentType = string.Empty;
            if (extensao.Equals(".pdf"))
                contentType = "application/pdf";

            if (extensao.Equals(".docx"))
                contentType = "application/docx";
            //FileInfo info = new FileInfo("out.html");
            // string createPath = Path.Combine(downloadPath, "out.html");
            using (Stream inputStream = System.IO.File.OpenRead(filePath))
            {
                FixedDocument doc;
                using (doc = new FixedDocument(inputStream))
                {
                    // create output file
                    using (TextWriter writer = info.CreateText())
                    {
                        // write returned html string to file
                        writer.Write(doc.Pages[0].ConvertToHtml(TextExtractionOptions.HtmlPage));

                    }
                }
            }
            Response.AppendHeader("Content-Disposition", contentType.ToString());


            return new FileStreamResult(stream, "application/pdf");
        }


        [HttpPost]
        public void FileUpload()
        {
            HttpPostedFileBase arquivo = Request.Files[0];
            int id = Convert.ToInt32(Request.Form[0]);
            Candidato candidato = new Candidato();
            string nome = Request.Form[1].ToString();
            string celular = Request.Form[2].ToString();
            string email = Request.Form[3].ToString();
            int idPerfil = Convert.ToInt32(Request.Form[4]);
            candidato.nome = nome;
            candidato.celular = celular;
            candidato.email = email;
            candidato.idPerfil = idPerfil;
            List<string> lista = new List<string>();
            lista.Add(".PDF");
            lista.Add(".DOC");
            lista.Add(".DOCX");

            var match = lista.FirstOrDefault(stringToCheck => stringToCheck.Contains(Path.GetExtension(arquivo.FileName).ToUpper()));
            var especialidade = Request.Form[5].Split(',').Select(s => Int32.Parse(s));
            if ((arquivo.ContentLength < 4096 * 1024) && (match != null))
            {
                string nomeArquivoGuid = Guid.NewGuid() + "." + Path.GetExtension(arquivo.FileName);

                var uploadPath = Server.MapPath("~/Upload");
                string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(nomeArquivoGuid));
                arquivo.SaveAs(caminhoArquivo);
                var candidatoUpdate = db.Candidatos.Find(id);
                if (candidatoUpdate != null)
                {
                    candidatoUpdate.curriculo = nomeArquivoGuid;
                    db.Candidatos.Attach(candidatoUpdate);
                    var entry = db.Entry(candidatoUpdate);
                    entry.Property(e => e.curriculo).IsModified = true;
                    db.SaveChanges();
                    var getValue = ConfigurationManager.AppSettings["emailVaga"];
                    MailMessage mail = new MailMessage();
                    mail.To.Add(getValue);
                    mail.From = new MailAddress("rhvagas@eginfo.com.br");
                    mail.Subject = "EGinfo -Cadastro de Candidato - Curriculo";
                    string Body = createEmailBody(candidato, especialidade);
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "email-ssl.com.br";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("rhvagas@eginfo.com.br", "eginfo01");
                    Attachment attachment = new Attachment(caminhoArquivo, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.FileName = Path.GetFileName(caminhoArquivo);
                    disposition.Size = new FileInfo(caminhoArquivo).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    smtp.Dispose();
                }
            }
            else
            {
                throw new Exception("Arquivo inválido");
            }
        }

        private string createEmailBody(Candidato candidato, IEnumerable<int> especialidade)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Template/HtmlTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{candidato}", candidato.nome);
            body = body.Replace("{telefone}", candidato.celular);
            body = body.Replace("{email}", candidato.email);
            body = body.Replace("{perfil}", ObterPerfil(candidato.idPerfil));

            var lista = (from d in db.Especialidades.ToList()
                         join e in especialidade on d.id equals e
                         select d.nome).ToList();

            string result = String.Join(", ", lista);
            body = body.Replace("{especialidades}", result.ToString());
            return body;
        }

        [HttpGet]
        public string ObterPerfil(int? id)
        {
            string nome = string.Empty;
            switch (id)
            {
                case 1:
                    nome = "Sênior";
                    break;
                case 2:
                    nome = "Pleno";
                    break;
                default:
                    nome = "Júnior";
                    break;
            }

            return nome;
        }
    }
}