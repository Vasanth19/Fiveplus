using System;
using System.Linq;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Repo;
using Fiveplus.Data.Uow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;


namespace Fiveplus.Tests.EF
{
    [TestClass]
    public class GigRepositoryTestManager
    {
        public GigRepositoryTestManager()
        {
            ReCreateCompleteDBForTesting();
        }

        private void ReCreateCompleteDBForTesting()
        {
            Database.SetInitializer(new DatabaseSeedingInitializer());
            using (var context = new FiveplusContext())
            {
                context.Database.Initialize(true);
                
            }

        }
        
        [TestMethod]
        public void CanRetrieveGigs()
        {

            using (var repo = new GigRepository(new ExplorerUow()))
            {

                bool addonStatus = repo.AllIncluding(g => g.AddonServices).Any(g => g.AddonServices.Count == 2); //Expect 2 records in Addon Services
                  
                Assert.IsTrue(addonStatus  );
            }
        }


        [TestMethod]
        public void CanInsertFullGraph()
        {
            
        }
    }
}
