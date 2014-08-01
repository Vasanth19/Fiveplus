using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.DBMappings;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;


// ReSharper disable once CheckNamespace
namespace Fiveplus.Data.DbContexts
{
    public class SalesContext : IdentityDbContext<ApplicationUser>
    {
        public SalesContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<SalesContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new OrderConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }

   }
