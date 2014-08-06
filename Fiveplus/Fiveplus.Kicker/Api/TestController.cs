using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace Fiveplus.Web.api
{
   public class TestController : ApiController
    {
        // GET api/gigs
        [Route("myapi/test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/gigs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/gigs
        public void Post([FromBody]string value)
        {
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
