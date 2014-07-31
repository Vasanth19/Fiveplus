using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DatabaseInitialization
{
    static class SqlServerMigrationHelper
    {
        internal static void SeedAppData(Fiveplus.Data.DbContexts.FiveplusContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Categories.AddOrUpdate(c => c.CategoryName,
                new Category { CategoryName = "Graphics" },
                new Category { CategoryName = "DigitalArt" },
                new Category { CategoryName = "Creative" }
                );

            context.Gigs.AddOrUpdate(g => g.Title,
                new Gig { Title = "I can write you a awesome Resume", CategoryId = 1, RowStatus = RowStatus.Enabled },
                new Gig { Title = "I can write you a awesome Cover Letter", CategoryId = 2, RowStatus = RowStatus.Enabled });

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }

            
        }


    }

    internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetCreatedUtcColumn(addColumnOperation.Column);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetCreatedUtcColumn(createTableOperation.Columns);

            base.Generate(createTableOperation);
        }

        private static void SetCreatedUtcColumn(IEnumerable<ColumnModel> columns)
        {
            foreach (var columnModel in columns)
            {
                SetCreatedUtcColumn(columnModel);
            }
        }

        private static void SetCreatedUtcColumn(PropertyModel column)
        {
            if (column.Name == "CreatedUtc" || column.Name == "Created" || column.Name == "LastModified")
            {
                column.DefaultValueSql = "GETUTCDATE()";
            }
        }
    }
}
