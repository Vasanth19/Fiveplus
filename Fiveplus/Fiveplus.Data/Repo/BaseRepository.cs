using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Helper;
using Fiveplus.Data.Models;
using Fiveplus.Data.Uow;

namespace Fiveplus.Data.Repo
{
    //http://code.msdn.microsoft.com/Repository-Pattern-in-MVC5-0bf41cd0
    public class BaseRepository<TEntity> : IEntityRepositoryAsync<TEntity> where TEntity : class 
    {
        protected DbSet<TEntity> DbSet; 
        protected readonly ExplorerContext context;

        public BaseRepository(ExplorerUow explorerUow) 
        {
            context = explorerUow.Context; 
            DbSet = context.Set<TEntity>(); 
        }

        public BaseRepository() 
        {
           // context = new ExplorerContext();
          //  DbSet = context.Set<TEntity>(); 
        } 
 
        public IQueryable<TEntity> All() 
        { 
            return DbSet; 
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual async Task<TEntity> FindAsync(int id) 
        { 
            return await DbSet.FindAsync(id); 
        } 
 

        /// <summary>
        /// Totally New Object or if something has changed in any of Gig's child records
        /// </summary>
        /// <param name="entity"></param>
        public virtual void InsertOrUpdateGraph(TEntity entity)
        {
            State _state = (State)entity.GetType().GetProperty("State").GetValue(entity);

            if (_state == State.Added)
            {
                DbSet.Add(entity);
            }
            else
            {
                DbSet.Add(entity);
                context.ApplyStateChanges();
            }
        }

        /// <summary>
        /// For Single Gig entity without the children
        /// </summary>
        /// <param name="entity"></param>
        public virtual void InsertOrUpdate(TEntity entity)
        {
            int _id = (int)entity.GetType().GetProperty("Id").GetValue(entity);
            if (_id == default(int))
            {
                // New entity
                context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                // Existing entity
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
        }
 
        public void DeleteAsync(TEntity entity) 
        { 
            DbSet.Remove(entity); 
        } 

        public void Dispose()
        {
            context.Dispose();
            
        }
}
}
