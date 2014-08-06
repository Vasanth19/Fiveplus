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
    public class Gig3Controller : ApiController
    {

        private IGigRepository _repo;

        public Gig3Controller(IGigRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Gig> Get(bool completeGraph = false)
        {
            IQueryable<Gig> results;
            if (completeGraph)
            {
                
                results = _repo.AllIncluding(c => c.Category, c=>c.Orders);
            }
            else
            {
                results = _repo.All;
            }

            return results.OrderByDescending(t => t.Created).Take(25).ToList();
        }

        public Gig Get(int id)
        {
            return _repo.Find(id);
        }
      
        // POST api/gigs
        public HttpResponseMessage Post([FromBody]Gig newGig)
        {
            
                newGig.Created = DateTime.UtcNow;
              
            if (true)
            {
                _repo.InsertOrUpdate(newGig);
             //   _repo.Save();
            
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
