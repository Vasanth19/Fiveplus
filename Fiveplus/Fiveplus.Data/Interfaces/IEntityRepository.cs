using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
    public interface IGigRepository :IEntityRepository<Gig>
    {
       
    }
// ReSharper disable once UnusedMember.Global
    public interface IOrderRepository :IEntityRepository<Order>
    {
       
    }

    public interface IEntityRepository<T> : IDisposable
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Gig Find(int id);
        void InsertOrUpdate(T entity);
        void Delete(int id);
     //   void Save();
    }
}
