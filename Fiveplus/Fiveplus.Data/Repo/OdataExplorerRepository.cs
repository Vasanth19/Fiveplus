using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breeze.ContextProvider.EF6;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
    public class OdataExplorerRepository : IOdataRepository
    {
        private readonly EFContextProvider<ExplorerContext>
          _contextProvider = new EFContextProvider<ExplorerContext>();

        private ExplorerContext _context { get { return _contextProvider.Context; } }

        public string Metadata
        {
            get { return _contextProvider.Metadata(); }
        }

        public Breeze.ContextProvider.SaveResult SaveChanges(Newtonsoft.Json.Linq.JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        public IQueryable<UserDetail> UserDetails()
        {
            return _context.UserDetails;
        }

        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
