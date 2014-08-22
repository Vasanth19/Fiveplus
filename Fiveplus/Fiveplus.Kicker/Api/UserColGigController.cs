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
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/UserCollectedGig")]
    public class UserCollectedGigController : ApiController
    {

        private IUserCollectedGigRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public UserCollectedGigController(ExplorerUow explorerUow, IUserCollectedGigRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<UserCollectedGig> Get(int gigId)
        {
            return _repo.All().Where(g => g.GigId == gigId).OrderByDescending(t => t.Created);
        }

         [Route("{id}", Name = "UserCollectedGigById")]
        [ResponseType(typeof(UserCollectedGig))]
        public async Task<IHttpActionResult> GetUserCollectedGig(int id)
        {
            UserCollectedGig UserCollectedGig = await _repo.FindAsync(id);
            if (UserCollectedGig == null)
            {
                return NotFound();
            }

            return Ok(UserCollectedGig);
        }

        // PUT: api/UserCollectedGig2Controller/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserCollectedGig(int id, [FromBody] UserCollectedGig userCollectedGig, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userCollectedGig.Id)
            {
                return BadRequest();
            }

             if(graph)
                 _repo.InsertOrUpdateGraph(userCollectedGig);
             else
                 _repo.InsertOrUpdate(userCollectedGig);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!UserCollectedGigExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("UserCollectedGigById", new { id = userCollectedGig.Id }, userCollectedGig);
            //return Ok("Updated UserCollectedGig");

        }

        // POST: api/UserCollectedGig2Controller
         [Route("")]
        [ResponseType(typeof(UserCollectedGig))]
        public async Task<IHttpActionResult> PostUserCollectedGig(UserCollectedGig userCollectedGig, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(userCollectedGig);
            else
                _repo.InsertOrUpdate(userCollectedGig);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("UserCollectedGigById", new { id = userCollectedGig.Id }, userCollectedGig);
        }


        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteUserCollectedGig(int id)
        {
            UserCollectedGig userCollectedGig = await _repo.FindAsync(id);
            if (userCollectedGig == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(userCollectedGig);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool UserCollectedGigExists(int id)
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
