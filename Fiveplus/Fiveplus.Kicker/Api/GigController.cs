using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using Fiveplus.Data;
using Fiveplus.Data.Interfaces;
using Fiveplus.Data.Models;
using Fiveplus.Data.Repo;
using Fiveplus.Data.Uow;
using Microsoft.AspNet.Identity;

namespace Fiveplus.Kicker.Api
{
    [RoutePrefix("api/gig")]
    public class GigController : ApiController
    {

        private IGigRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public GigController(ExplorerUow explorerUow, IGigRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<Gig> GetGigs()
        {
            return _repo.All().Include(c => c.AddonServices);
        }

         [Route("{id}", Name = "GigById")]
        [ResponseType(typeof(Gig))]
        public async Task<IHttpActionResult> GetGig(int id)
        {
            User.Identity.GetUserId();

            Gig gig = await _repo.FindAsync(id);
            if (gig == null)
            {
                return NotFound();
            }

            return Ok(gig);
        }

        // PUT: api/Gig2Controller/5
         [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGig(int id, [FromBody] Gig gig, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gig.Id)
            {
                return BadRequest();
            }

             if(graph)
                _repo.InsertOrUpdateGraph(gig);
             else
                 _repo.InsertOrUpdate(gig);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!GigExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("GigById", new { id = gig.Id }, gig);
            //return Ok("Updated Gig");

        }

        // POST: api/Gig2Controller
         [Route("")]
        [ResponseType(typeof(Gig))]
         public async Task<IHttpActionResult> PostGig(Gig gig, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid && gig.Id == default(int))
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(gig);
            else
                _repo.InsertOrUpdate(gig);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("GigById", new { id = gig.Id }, gig);
        }

        // DELETE: api/Gig2Controller/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteGig(int id)
        {
            Gig gig = await _repo.FindAsync(id);
            if (gig == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(gig);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool GigExists(int id)
        {
            return _repo.All().Count(g => g.Id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _explorerUow.Dispose();
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }

      
    }
}
