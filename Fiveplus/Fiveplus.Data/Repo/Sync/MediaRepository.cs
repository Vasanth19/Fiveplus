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
    public class MediaRepository : IMediaRepository
    {
        private ExplorerContext context;

        public MediaRepository(ExplorerUow explorerUow)
        {
            context = explorerUow.Context;
        }

        // Used for automated testing
        public MediaRepository()
        {
            context = new ExplorerContext();
        }

        public IQueryable<Media> All
        {
            get { return context.MediaUrls; }
        }

        public IQueryable<Media> GigMedia(int gigId)
        {
             return context.MediaUrls.Where(g=>g.GigId == gigId);
        }

        public IQueryable<Media> AllIncluding(params Expression<Func<Media, object>>[] includeProperties)
        {
            IQueryable<Media> query = context.MediaUrls;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Media Find(int id)
        {
            return context.MediaUrls.Find(id);
        }

  /// <summary>
        /// For Single Gig entity without the children
        /// </summary>
        /// <param name="gig"></param>
        public void InsertOrUpdate(Media media)
        {
            if (media.Id == default(int)) {
                // New entity
                context.Entry(media).State = System.Data.Entity.EntityState.Added;
            } else {
                // Existing entity
                context.Entry(media).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

      

    }

    
}