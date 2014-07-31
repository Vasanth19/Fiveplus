using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Repo;

namespace Fiveplus.Data.Helper
{
    public static class StateHelpers
    {
        public static EntityState ConverState(State state)
        {
            switch (state)
            {
                    case State.Added: return EntityState.Added;
                    case State.Modified: return EntityState.Modified;
                    case State.Deleted: return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        public static void ApplyStateChanges(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<IBaseModel>())
            {
                IBaseModel stateInfo = entry.Entity;
                entry.State = ConverState(stateInfo.State);
            }
        }
    }


}
