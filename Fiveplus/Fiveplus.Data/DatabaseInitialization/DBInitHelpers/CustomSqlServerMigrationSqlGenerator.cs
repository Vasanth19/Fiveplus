using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DatabaseInitialization
{

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
