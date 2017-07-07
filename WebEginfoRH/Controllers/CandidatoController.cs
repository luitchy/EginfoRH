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
        

        // GET: Candidato
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
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
        public JsonResult GetCEP(string cep)
        {
            return null;
        }

        [HttpGet]
        public JsonResult GetEspecialidade()
        {
            EGINFO_RHContext db = new EGINFO_RHContext();
            var lista = db.tb_Especialidade.Select(n => n).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}