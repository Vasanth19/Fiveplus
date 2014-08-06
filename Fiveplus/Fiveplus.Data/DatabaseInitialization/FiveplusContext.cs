using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.DatabaseInitialization;
using Fiveplus.Data.DBMappings;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;


// ReSharper disable once CheckNamespace
namespace Fiveplus.Data.DbContexts
{
    /// <summary>
    /// Servers as a entire database context. Would be used only for testing and DB Initialization
    /// </summary>
    public class FiveplusContext : IdentityDbContext<ApplicationUser>
    {
        public FiveplusContext()
            : base("DefaultConnection")
        {
        }
     
        // User Tables
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserCollectedGig> UserCollectedGigs { get; set; }
        public DbSet<UserInboxMessage> UserInboxMessages { get; set; }

        //Gig Related Tables
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Media> MediaUrls { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Sales Related Tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        


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

    /// <summary>
    /// Used in the startup.cs in Test Projects to DropCreateDatabaseAlways
    /// Does not use configuration.cs
    /// </summary>
    public class DatabaseSeedingInitializer : DropCreateDatabaseAlways<FiveplusContext>
    {
        protected override void Seed(FiveplusContext context)
        {
            SqlServerMigrationHelper.SeedAppData(context);
            
        }
    }
   }
