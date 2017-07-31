﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebEginfoRH.Models;

namespace WebApiEginfo.Controllers
{
    public class CandidatoController : ApiController
    {
        private EGINFORHContext db = new EGINFORHContext();
        
        [AcceptVerbs("GET", "POST")]
        public List<Candidato> Buscar(int idPerfil, [FromUri]List<int> especialidade)
        {
            EGINFORHContext db = new EGINFORHContext();

            db.Configuration.ProxyCreationEnabled = false;
            var result = (
             from a in db.Especialidades
             from b in a.Candidatos
             join c in db.Candidatos on b.id equals c.id
             where especialidade.Contains(a.id)
             select new
             {
                 id = c.id,
                 nome = c.nome,
                 curriculo = c.curriculo,
                 idPerfil = c.idPerfil
             }).ToList().Distinct()
             .Select(x => new Candidato()
             {
                 id = x.id,
                 nome = x.nome,
                 curriculo = x.curriculo,
                 idPerfil = x.idPerfil
             });

            if (idPerfil != 0)
            {
                result = result.Where(c => c.idPerfil == idPerfil);
            }
            return result.ToList();
        }

        [ResponseType(typeof(List<Especialidade>))]
        public List<Especialidade> GetEspecialidades()
        {
            EGINFORHContext db = new EGINFORHContext();
            var lista =  db.Especialidades.Select(n => n).ToList();
            return lista;
        }
        // GET: api/Candidato
        public IQueryable<Candidato> GetCandidatos()
        {
            return db.Candidatos;
        }

        // GET: api/Candidato/5
        [ResponseType(typeof(Candidato))]
        public IHttpActionResult GetCandidato(int id)
        {
            Candidato candidato = db.Candidatos.Find(id);
            if (candidato == null)
            {
                return NotFound();
            }

            return Ok(candidato);
        }

        // PUT: api/Candidato/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCandidato(int id, Candidato candidato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != candidato.id)
            {
                return BadRequest();
            }

            db.Entry(candidato).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidatoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Candidato
        [ResponseType(typeof(Candidato))]
        public IHttpActionResult PostCandidato(Candidato candidato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Candidatos.Add(candidato);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = candidato.id }, candidato);
        }

        // DELETE: api/Candidato/5
        [ResponseType(typeof(Candidato))]
        public IHttpActionResult DeleteCandidato(int id)
        {
            Candidato candidato = db.Candidatos.Find(id);
            if (candidato == null)
            {
                return NotFound();
            }

            db.Candidatos.Remove(candidato);
            db.SaveChanges();

            return Ok(candidato);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CandidatoExists(int id)
        {
            return db.Candidatos.Count(e => e.id == id) > 0;
        }
    }
}