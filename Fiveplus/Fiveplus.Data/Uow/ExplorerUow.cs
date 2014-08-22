using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Interfaces;

namespace Fiveplus.Data.Uow
{
    public class ExplorerUow: IUnitOfWork<ExplorerContext>
    {
        private readonly ExplorerContext _context;
        public ExplorerContext Context
        {
            get { return _context; }
        }

        public ExplorerUow()
        {
            _context = new ExplorerContext();
        }
      
        //For Automated testing;
        public ExplorerUow(ExplorerContext context)
        {
            _context = context;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
