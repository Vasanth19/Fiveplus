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
    public class ExplorerContext : IdentityDbContext<ApplicationUser>
    {
        //Access All the items including Sales tables
        public ExplorerContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ExplorerContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new GigConfiguration());
            modelBuilder.Configurations.Add(new MediaConfiguration());
            
            modelBuilder.Configurations.Add(new UserDetailConfiguration());
            modelBuilder.Configurations.Add(new UserInboxConfiguration());
            modelBuilder.Configurations.Add(new UserCollectedGigConfiguration());
 
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Entity<Category>();

            base.OnModelCreating(modelBuilder);
        }


        // User Tables, //Going to impemente no tracking in repository as the intention is readonly
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserCollectedGig> UserCollectedGigs { get; set; }
        public DbSet<UserInboxMessage> UserInboxMessages { get; set; }

        //Gig Related Tables
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Media> MediaUrls { get; set; }
        public DbSet<Category> Categories { get; set; }

        //Sales Related Tables , //Going to impemente no tracking in repository as the intention is readonly
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }

   }
