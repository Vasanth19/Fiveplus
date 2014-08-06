using System.Data.Entity;
using Fiveplus.Data.DbContexts;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fiveplus.Portal.Startup))]
namespace Fiveplus.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // Database.SetInitializer(new DropCreateDatabaseAlways<FiveplusContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FiveplusContext, Fiveplus.Data.DatabaseInitialization.Configuration>());
            ConfigureAuth(app);
        }
    }
}
