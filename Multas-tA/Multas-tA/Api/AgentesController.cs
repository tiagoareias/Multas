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
using Multas_tA.Models;

namespace Multas_tA.Api
{
    public class AgentesController : ApiController
    {
        // Referência para a base de dados.
        private MultasDb db = new MultasDb();

        // CRUD: Obter uma lista de Agentes
        // GET: api/Agentes
        public IHttpActionResult GetAgentes()
        {
            var resultado = db.Agentes
                .Select(agente => new
                {
                    agente.ID,
                    agente.Nome,
                    agente.Esquadra,
                    agente.Fotografia
                })
                .ToList();
            
            return Ok(resultado);
        }

        // CRUD: Obter um agente, através do seu ID.
        // - Se o agente não existe -> 404 (Not Found)
        // GET: api/Agentes/5
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult GetAgentes(int id)
        {
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return NotFound();
            }

            return Ok(agentes);
        }

        // CRUD: Atualizar (PUT) um agente, através do seu ID.
        // - Se o agente não é válido (validações do MVC) -> 400 (Bad Request)
        // - Se o agente não existe -> 404 (Not Found)
        // PUT: api/Agentes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgentes(int id, Agentes agentes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agentes.ID)
            {
                return BadRequest();
            }

            db.Entry(agentes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentesExists(id))
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

        // CRUD: Criar um agente.
        // - Se o agente não é válido (validações do MVC) -> 400 (Bad Request)
        // - Se estiver tudo OK -> 201 (Created) com o objeto do Agente.
        // POST: api/Agentes
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult PostAgentes(Agentes agentes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Agentes.Add(agentes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AgentesExists(agentes.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = agentes.ID }, agentes);
        }

        // CRUD: Apagar um agente
        // - Se o agente não existe -> 404 (Not Found)
        // DELETE: api/Agentes/5
        [ResponseType(typeof(Agentes))]
        public IHttpActionResult DeleteAgentes(int id)
        {
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return NotFound();
            }

            db.Agentes.Remove(agentes);
            db.SaveChanges();

            return Ok(agentes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgentesExists(int id)
        {
            return db.Agentes.Count(e => e.ID == id) > 0;
        }
    }
}