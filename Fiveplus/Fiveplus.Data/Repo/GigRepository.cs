using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Helper;
using Fiveplus.Data.Models;
using Fiveplus.Data.Uow;

namespace Fiveplus.Data.Repo
{ 
    public class GigRepository : IGigRepository
    {
        private ExplorerContext context;

        public GigRepository(ExplorerUow explorerUow)
        {
            context = explorerUow.Context;
        }

        // Used for automated testing
        public GigRepository()
        {
            context = new ExplorerContext();
        }

        public IQueryable<Gig> All
        {
            get { return context.Gigs; }
        }

        public IQueryable<Gig> AllIncluding(params Expression<Func<Gig, object>>[] includeProperties)
        {
            IQueryable<Gig> query = context.Gigs;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Gig Find(int id)
        {
            return context.Gigs.Find(id);
        }

        /// <summary>
        /// Totally New Object or if something has changed in any of Gig's child records
        /// </summary>
        /// <param name="gigGraph"></param>
        public void InsertOrUpdateGraph(Gig gigGraph)
        {
            if (gigGraph.State == Repo.State.Added)
            {
                context.Gigs.Add(gigGraph);
            }
            else
            {
                context.Gigs.Add(gigGraph);
                context.ApplyStateChanges();
            }
        }

        /// <summary>
        /// For Single Gig entity without the children
        /// </summary>
        /// <param name="gig"></param>
        public void InsertOrUpdate(Gig gig)
        {
            if (gig.Id == default(int)) {
                // New entity
                context.Entry(gig).State = System.Data.Entity.EntityState.Added;
            } else {
                // Existing entity
                context.Entry(gig).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var gig = context.Gigs.Find(id);
            context.Gigs.Remove(gig);
        }

        //public void Save()
        //{
        //    context.SaveChanges();
        //}

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    
}