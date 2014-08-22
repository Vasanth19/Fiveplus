using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.Repo
{
  public interface IBaseModel
    {
      DateTime? Created { get; set; }
      State State { get; set; }
    }

    public enum State
    {
        Unchanged = 0,
        Added,
        Modified,
        Deleted
    }
}
