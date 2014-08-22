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
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/UserInboxMessage")]
    public class UserInboxMessageController : ApiController
    {

        private IUserInboxMessageRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public UserInboxMessageController(ExplorerUow explorerUow, IUserInboxMessageRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<UserInboxMessage> Get(int gigId)
        {
            return _repo.All();
        }

         [Route("{id}", Name = "UserInboxMessageById")]
        [ResponseType(typeof(UserInboxMessage))]
        public async Task<IHttpActionResult> GetUserInboxMessage(int id)
        {
            UserInboxMessage userInboxMessage = await _repo.FindAsync(id);
            if (userInboxMessage == null)
            {
                return NotFound();
            }

            return Ok(userInboxMessage);
        }

        // PUT: api/UserInboxMessage2Controller/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
         public async Task<IHttpActionResult> PutUserInboxMessage(int id, [FromBody] UserInboxMessage userInboxMessage, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInboxMessage.Id)
            {
                return BadRequest();
            }

             if(graph)
                 _repo.InsertOrUpdateGraph(userInboxMessage);
             else
                 _repo.InsertOrUpdate(userInboxMessage);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!UserInboxMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("UserInboxMessageById", new { id = userInboxMessage.Id }, userInboxMessage);
            //return Ok("Updated UserInboxMessage");

        }

        // POST: api/UserInboxMessage2Controller
         [Route("")]
        [ResponseType(typeof(UserInboxMessage))]
        public async Task<IHttpActionResult> PostUserInboxMessage(UserInboxMessage userInboxMessage, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(userInboxMessage);
            else
                _repo.InsertOrUpdate(userInboxMessage);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("UserInboxMessageById", new { id = userInboxMessage.Id }, userInboxMessage);
        }


        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteUserInboxMessage(int id)
        {
            UserInboxMessage userInboxMessage = await _repo.FindAsync(id);
            if (userInboxMessage == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(userInboxMessage);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool UserInboxMessageExists(int id)
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
