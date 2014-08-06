using Fiveplus.Data.DbContexts;
using Owin;
using System.Data.Entity;

namespace IdentitySample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FiveplusContext, Fiveplus.Data.DatabaseInitialization.Configuration>());
            ConfigureAuth(app);
        }
    }
}
