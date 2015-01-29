using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
    /*
     *  // User Tables
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserCollectedGig> UserCollectedGigs { get; set; }
        public DbSet<UserInboxMessage> UserInboxMessages { get; set; }

        //Gig Related Tables
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Media> MediaUrls { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Sales Related Tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
     */



    public interface IUserInboxMessageRepositoryAsync : IEntityRepositoryAsync<UserInboxMessage> { }
    public interface IUserCollectedGigRepositoryAsync : IEntityRepositoryAsync<UserCollectedGig> { }

    public interface IUserDetailRepositoryAsync : IEntityRepositoryAsync<UserDetail>
    {
        Task<UserDetail> FindAsync(string id);
    }

    public interface ICategoryRepositoryAsync : IEntityRepositoryAsync<Category> { }
    public interface IAddonRepositoryAsync : IEntityRepositoryAsync<Addon> { }
    public interface IMediaRepositoryAsync : IEntityRepositoryAsync<Media>{}
    public interface IGigRepositoryAsync : IEntityRepositoryAsync<Gig> { }

    public interface IEntityRepositoryAsync<T> : IDisposable
    {
        IQueryable<T> All();
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindAsync(int id);
        void DeleteAsync(T entity);
        void InsertOrUpdateGraph(T entity);
        void InsertOrUpdate(T entity);

    }
}
