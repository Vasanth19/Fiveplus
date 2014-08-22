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
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/Addon")]
    public class AddonController : ApiController
    {

        private IAddonRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public AddonController(ExplorerUow explorerUow, IAddonRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<Addon> Get(int gigId)
        {
            return _repo.All().Where(g => g.GigId == gigId).OrderByDescending(t => t.Created);
        }

         [Route("{id}", Name = "AddonById")]
        [ResponseType(typeof(Addon))]
        public async Task<IHttpActionResult> GetAddon(int id)
        {
            Addon Addon = await _repo.FindAsync(id);
            if (Addon == null)
            {
                return NotFound();
            }

            return Ok(Addon);
        }

        // PUT: api/Addon2Controller/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddon(int id, [FromBody] Addon addon, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != addon.Id)
            {
                return BadRequest();
            }

             if(graph)
                _repo.InsertOrUpdateGraph(addon);
             else
                 _repo.InsertOrUpdate(addon);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!AddonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("AddonById", new { id = addon.Id }, addon);
            //return Ok("Updated Addon");

        }

        // POST: api/Addon2Controller
         [Route("")]
        [ResponseType(typeof(Addon))]
         public async Task<IHttpActionResult> PostAddon(Addon addon, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(addon);
            else
                _repo.InsertOrUpdate(addon);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("AddonById", new { id = addon.Id }, addon);
        }


        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteAddon(int id)
        {
            Addon Addon = await _repo.FindAsync(id);
            if (Addon == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(Addon);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool AddonExists(int id)
        {
            return _repo.All().Count(m => m.Id == id) > 0;
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
