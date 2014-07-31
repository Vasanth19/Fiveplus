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
    public class FiveplusContext : IdentityDbContext<ApplicationUser>
    {
        public FiveplusContext()
            : base("DefaultConnection")
        {
        }
     
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new GigConfiguration());

            modelBuilder.Configurations.Add(new IdentityUserConfiguration());
            modelBuilder.Configurations.Add(new UserInboxConfiguration());
            modelBuilder.Configurations.Add(new UserCollectedGigConfiguration());

            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Entity<Category>();

            base.OnModelCreating(modelBuilder);
        }
    }

   }
