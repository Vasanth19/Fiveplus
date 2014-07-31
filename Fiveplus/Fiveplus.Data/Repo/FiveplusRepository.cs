using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
    public class FiveplusRepository : IFiveplusRepository
    {
        private FiveplusContext _ctx;
        public FiveplusRepository(FiveplusContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<Gig> GetGigs()
        {
            return _ctx.Gigs;
        }

        public IQueryable<Gig> GetGigswithAddon()
        {
            throw new NotImplementedException();
        }

        public bool AddGig(Gig newGig)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}