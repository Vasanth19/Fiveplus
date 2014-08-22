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
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/media")]
    public class MediaController : ApiController
    {

        private IMediaRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public MediaController(ExplorerUow explorerUow, IMediaRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<Media> Get(int gigId)
        {
            return _repo.All().Where(g => g.GigId == gigId).OrderByDescending(t => t.Created);
        }

         [Route("{id}", Name = "MediaById")]
        [ResponseType(typeof(Media))]
        public async Task<IHttpActionResult> GetMedia(int id)
        {
            Media media = await _repo.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            return Ok(media);
        }

        // PUT: api/Media2Controller/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMedia(int id, [FromBody] Media media, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != media.Id)
            {
                return BadRequest();
            }

             if(graph)
                _repo.InsertOrUpdateGraph(media);
             else
                 _repo.InsertOrUpdate(media);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!MediaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("MediaById", new { id = media.Id }, media);
            //return Ok("Updated Media");

        }

        // POST: api/Media2Controller
         [Route("")]
        [ResponseType(typeof(Media))]
         public async Task<IHttpActionResult> PostMedia(Media media, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(media);
            else
                _repo.InsertOrUpdate(media);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("MediaById", new { id = media.Id }, media);
        }


        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteMedia(int id)
        {
            Media Media = await _repo.FindAsync(id);
            if (Media == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(Media);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool MediaExists(int id)
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
