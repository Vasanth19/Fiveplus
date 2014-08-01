namespace Fiveplus.Data.DatabaseInitialization
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Used in the startup.cs in Database.SetInitializer(new MigrateDatabaseToLatestVersion<FiveplusContext, Fiveplus.Data.DatabaseInitialization.Configuration>());
    /// </summary>
    public sealed class Configuration : DbMigrationsConfiguration<Fiveplus.Data.DbContexts.FiveplusContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"DatabaseInitialization\FiveplusMigrations";
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(Fiveplus.Data.DbContexts.FiveplusContext context)
        {
            //  This method will be called after migrating to the latest version.
             // #if DEBUG
                SqlServerMigrationHelper.SeedAppData(context);
            // #endif

        }
    }

   
}