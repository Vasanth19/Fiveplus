using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fiveplus.Data;
using Fiveplus.Data.Models;
using Fiveplus.Data.Repo;

namespace Fiveplus.Portal.Api
{
    [RoutePrefix("api")]
    public class GigController : ApiController
    {

        private IFiveplusRepository _repo;

        public GigController(IFiveplusRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Gig> Get(bool includeReplies = false)
        {
            IQueryable<Gig> results;
            if (includeReplies)
            {
                results = _repo.GetGigs();
            }
            else
            {
                results = _repo.GetGigs();
            }

            return results.OrderByDescending(t => t.Created).Take(25).ToList();
        }

        public Gig Get(int id)
        {
            return _repo.GetGigs().SingleOrDefault(t => t.Id == id);
        }
      
        // POST api/gigs
        public HttpResponseMessage Post([FromBody]Gig newGig)
        {
            if (newGig.Created == default(DateTime))
            {
                newGig.Created = DateTime.UtcNow;
            }

            if (_repo.AddGig(newGig) && _repo.Save())
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, newGig);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        // PUT api/gigs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/gigs/5
        public void Delete(int id)
        {
        }
    }
}
