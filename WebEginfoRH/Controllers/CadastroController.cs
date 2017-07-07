using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEginfoRH.Models;

namespace WebEginfoRH.Controllers
{
    public class CadastroController : Controller
    {
        // GET: Cadastro
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateCustomer(tb_Candidato model)
        {
            if (ModelState.IsValid)
            {
                //Data save to database
                var RedirectUrl = Url.Action("Cadastro1");
                //Teste Everton Pascoal Silv
                return Json(new { success = true, redirectUrl = RedirectUrl });
            }
            return Json(new
            {
                success = false,
                errors = ModelState.Keys.SelectMany(i => ModelState[i].Errors).Select(m => m.ErrorMessage).ToArray()
            });
        }

        // GET: Cadastro/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cadastro/Create
        public ActionResult Create()
        {
            return View("Cadastro1");
        }

        // POST: Cadastro/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cadastro/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cadastro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cadastro/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cadastro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
