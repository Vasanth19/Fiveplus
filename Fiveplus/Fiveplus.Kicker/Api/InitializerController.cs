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
    [System.Web.Http.RoutePrefix("api/Initializer")]
    public class InitializerController : ApiController
    {

        private ICategoryRepositoryAsync _repo;
        private ExplorerUow _explorerUow;

        public InitializerController(ExplorerUow explorerUow, ICategoryRepositoryAsync repo)
        {
            _explorerUow = explorerUow;
            _repo = repo;
        }


        [Route("categories")]
        public IQueryable<Category> Get()
        {
            return _repo.All();
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
