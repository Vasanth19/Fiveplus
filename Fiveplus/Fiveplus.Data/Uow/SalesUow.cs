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
    public class SalesUow: IUnitOfWork<SalesContext>
    {
        private readonly SalesContext _context;

        public SalesUow()
        {
            _context = new SalesContext();
        }

        //For Automated testing;
        public SalesUow(SalesContext context)
        {
            _context = context;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public SalesContext Context
        {
            get { return _context; }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
