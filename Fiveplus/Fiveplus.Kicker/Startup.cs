using Fiveplus.Data.DbContexts;
using Owin;
using System.Data.Entity;

namespace Fiveplus.Kicker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<FiveplusContext, Fiveplus.Data.DatabaseInitialization.Configuration>());
            //Database.SetInitializer<FiveplusContext>(new DatabaseSeedingInitializer());

            Database.SetInitializer(new DatabaseSeedingInitializer());
            using (var context = new FiveplusContext())
            {
                context.Database.Initialize(true);
            }


            ConfigureAuth(app);
        }
    }
}
