using System;
using System.Linq;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
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
            AppDomain.CurrentDomain.SetData("DataDirectory", @"C:\BeOrganized\Projects\Github\Fiveplus\Fiveplus\Fiveplus.Kicker\App_Data\");
            //AppDomain.CurrentDomain.SetData("DataDirectory", @"C:\Projects\GitHub\Fiveplus\Fiveplus\Fiveplus.Kicker\App_Data\");
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

                bool addonStatus = repo.AllIncluding(g => g.AddonServices).Any(g => g.AddonServices.Count == 2); //Expect 2 records in Addon Services after seed
                  
                Assert.IsTrue(addonStatus  );
            }
        }


        [TestMethod]
        public void CanRetrieveandModifyUserDetails()
        {
            var _explorerUow = new ExplorerUow();
            using (var repo = new UserDetailRepositoryAsync(_explorerUow))
            {

                UserDetail user = repo.FindByUserId("da063249-4b43-48d9-8875-96477180e290");

                user.FullName = "Vasanth Subramanyam";
                repo.InsertOrUpdate(user);
                _explorerUow.Save();
                
                Assert.IsTrue(true);
            }
        }
        [TestMethod]
        public void CanCreateUserDetails()
        {
            var _explorerUow = new ExplorerUow();
            using (var repo = new UserDetailRepositoryAsync(_explorerUow))
            {

                UserDetail user = new UserDetail() {FullName = "My Full Name",Preference = NotificationPreference.Weekly};

                repo.InsertOrUpdate(user);
                _explorerUow.Save();

                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void CanInsertFullGraph()
        {
            
        }
    }
}
