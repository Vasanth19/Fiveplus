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
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/UserDetail")]
    public class UserDetailController : ApiController
    {

        private IUserDetailRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public UserDetailController(ExplorerUow explorerUow, IUserDetailRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("")]
        public IQueryable<UserDetail> Get(int gigId)
        {
            return _repo.All();
        }

         [Route("{id}", Name = "UserDetailById")]
        [ResponseType(typeof(UserDetail))]
        public async Task<IHttpActionResult> GetUserDetail(int id)
        {
            UserDetail userDetail = await _repo.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }

        // PUT: api/UserDetail2Controller/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserDetail(string id, [FromBody] UserDetail userDetail, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDetail.UserId)
            {
                return BadRequest();
            }

             if(graph)
                 _repo.InsertOrUpdateGraph(userDetail);
             else
                 _repo.InsertOrUpdate(userDetail);

            try
            {
                await _explorerUow.SaveAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!UserDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    BadRequest(e.Message + e.ToString());
                }
            }

            //  return StatusCode(HttpStatusCode.OK);
            return CreatedAtRoute("UserDetailById", new { id = userDetail.UserId }, userDetail);
            //return Ok("Updated UserDetail");

        }

        // POST: api/UserDetail2Controller
         [Route("")]
        [ResponseType(typeof(UserDetail))]
        public async Task<IHttpActionResult> PostUserDetail(UserDetail userDetail, [FromUri] bool graph = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (graph)
                _repo.InsertOrUpdateGraph(userDetail);
            else
                _repo.InsertOrUpdate(userDetail);

            await _explorerUow.SaveAsync();

            return CreatedAtRoute("UserDetailById", new { id = userDetail.UserId}, userDetail);
        }


        [Route("{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DeleteUserDetail(int id)
        {
            UserDetail userDetail = await _repo.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            _repo.DeleteAsync(userDetail);
            await _explorerUow.SaveAsync();

            return Ok();
        }

        private bool UserDetailExists(string id)
        {
            return _repo.All().Count(m => m.UserId == id) > 0;
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
