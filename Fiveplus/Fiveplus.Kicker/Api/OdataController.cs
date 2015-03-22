using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
using Fiveplus.Data.Repo;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Breeze.ContextProvider;


namespace Fiveplus.Kicker.Api
{
    [Authorize]
    [BreezeController]
    public class OdataController : ApiController
    {
        private OdataExplorerRepository _repository;
        public OdataController(OdataExplorerRepository repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public string Metadata()
        {
            return _repository.Metadata;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _repository.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<UserDetail> UserDetails()
        {
            var userId = User.Identity.GetUserId();
            return _repository.UserDetails().Where(u=>u.UserId == userId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }


    }
}