using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Helper;
using Fiveplus.Data.Models;
using Fiveplus.Data.Uow;

namespace Fiveplus.Data.Repo
{
    public class GigRepositoryAsync :BaseRepository<Gig>,IGigRepositoryAsync
    {
        /// <summary>
        /// Very much reuired for DI to work
        /// </summary>
        /// <param name="explorerUow"></param>
        public GigRepositoryAsync(ExplorerUow explorerUow) :base (explorerUow)
        {
        }

        public IQueryable<Gig> SearchFor(Expression<Func<Gig, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

    }
}
