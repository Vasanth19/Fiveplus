using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
    public interface IFiveplusRepository
    {
        IQueryable<Gig> GetGigs();
        IQueryable<Gig> GetGigswithAddon();

        bool AddGig(Gig newGig);

        bool Save();
    }

   
}
