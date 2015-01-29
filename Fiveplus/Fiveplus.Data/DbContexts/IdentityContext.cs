using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Fiveplus.Data.DatabaseInitialization;
using Fiveplus.Data.DBMappings;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fiveplus.Data.DbContexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
            : base("DefaultConnection")
        {
            //No Seeding necessary as the DB is created using FiveplusContext
            Database.SetInitializer<IdentityContext>(null);
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetail>();
            modelBuilder.Configurations.Add(new UserDetailConfiguration());
            modelBuilder.Ignore<UserCollectedGig>();
            modelBuilder.Ignore<Gig>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<UserInboxMessage>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDetail> UserDetails { get; set; }


        public static IdentityContext Create()
        {
            return new IdentityContext();
        }
    }


}