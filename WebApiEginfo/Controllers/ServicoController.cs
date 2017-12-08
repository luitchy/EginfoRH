using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
    public class ServicoController : ApiController
    {
        private EGINFORHContext db = new EGINFORHContext();

      
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
        [HttpPost]
      //  public int Salvar( Candidato candidato,ICollection<int> especialidade,  Endereco endereco)
       public int Salvar(JObject objData)
        {

            // Candidato candidato = request.Content.ReadAsAsync<Candidato>().Result;
            //   ICollection<int> especialidade =  request.Content.ReadAsAsync(List<int>).ToList();
            // Endereco endereco = request.Content.ReadAsAsync<Endereco>().Result;
            dynamic jsonData = objData;
            JObject candidatoJson = jsonData.candidato;
            JArray especialidade = jsonData.especialidade;
            JObject enderecoJson = jsonData.endereco;
            var candidato = candidatoJson.ToObject<Candidato>();
            var endereco = enderecoJson.ToObject<Endereco>();
           // int idPerfil = jsonData["idPerfil"].ToObject<int>();
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