using Apitron.PDF.Kit;
using Apitron.PDF.Kit.Extraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApiEginfo.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
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
    }
}