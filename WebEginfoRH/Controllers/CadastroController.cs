using System;

using System.Net.Http;

using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebEginfoRH.Models;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;

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

        public async Task<JsonResult> GetEspecialidade()
        {
           
            EGINFORHContext db = new EGINFORHContext();          
            var lista =  await db.Especialidades.ToListAsync();
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
        public void FileUpload()
        {
            HttpPostedFileBase arquivo = Request.Files[0];
            int id = Convert.ToInt32(Request.Form[0]);
            Candidato candidato = new Candidato();            
            string nome = Request.Form[1].ToString();
            string celular = Request.Form[2].ToString();
            string email = Request.Form[3].ToString();
            int idPerfil = Convert.ToInt32(Request.Form[4]);
            //ICollection<int> especialida = Request.Form[5].ToList();
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
        /*
        [HttpPost]
        public void EnviarEmail(Candidato candidato, ICollection<int> especialidade, Endereco endereco, string arquivoTo)
        {
           
            var uploadPath = Server.MapPath("~/Upload");
            string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivoTo));
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
        */
     
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
