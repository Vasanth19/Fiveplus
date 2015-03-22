using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Breeze.ContextProvider;
using Fiveplus.Data.Models;
using Newtonsoft.Json.Linq;

namespace Fiveplus.Data.Repo
{
    
    public interface IOdataRepository : IDisposable
    {
        string Metadata { get; }
        SaveResult SaveChanges(JObject saveBundle);
        
    }

}
