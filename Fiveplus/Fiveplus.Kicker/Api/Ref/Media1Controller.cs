using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Fiveplus.Data;
using Fiveplus.Data.Models;
using Fiveplus.Data.Repo;

namespace Fiveplus.Kicker.Api
{
    [System.Web.Http.RoutePrefix("api/gigs/{gigId}/media1")]
    public class Media1Controller : ApiController
    {

      private IMediaRepository _repo;

      public Media1Controller(IMediaRepository repo)
        {
            _repo = repo;
        }

        [System.Web.Http.Route("")]
        public IEnumerable<Media> Get(int gigId)
        {
            IQueryable<Media> results;
            results = _repo.All.Where(g => g.GigId == gigId);

            return results.OrderByDescending(t => t.Created).Take(25).ToList();
        }

        [System.Web.Http.Route("{id}")]
        public Media GetMedia(int id)
        {
            return _repo.Find(id);
        }

        [System.Web.Http.Route("")]
        // POST api/Medias
        public HttpResponseMessage Post([FromBody]Media newMedia)
        {
            
                newMedia.Created = DateTime.UtcNow;
              
            if (true)
            {
                _repo.InsertOrUpdate(newMedia);
             //   _repo.Save();
            
                return Request.CreateResponse(HttpStatusCode.Accepted, newMedia);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        [System.Web.Http.Route("{id}")]
        // PUT api/Medias/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [System.Web.Http.Route("{id}")]
        // DELETE api/Medias/5
        public void Delete(int id)
        {
        }
    }
}
