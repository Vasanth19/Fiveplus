using System;
using System.Linq;
using Fiveplus.Data.Repo;
using Fiveplus.Data.Uow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fiveplus.Tests.EF
{
    [TestClass]
    public class GigRepositoryTestManager
    {
        [TestMethod]
        public void CanRetrieveGigs()
        {
            using (var repo = new GigRepository(new UowExplorer()))
            {
                Assert.IsTrue(
                    repo.All.Any(g=>g.Id>0)
                   );
            }
        }
    }
}
