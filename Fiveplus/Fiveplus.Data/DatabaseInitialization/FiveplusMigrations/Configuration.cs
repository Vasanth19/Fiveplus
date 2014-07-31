using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DatabaseInitialization
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Fiveplus.Data.DbContexts.FiveplusContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"DbContexts\FiveplusMigrations";
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
