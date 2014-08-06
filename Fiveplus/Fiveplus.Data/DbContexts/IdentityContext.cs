using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fiveplus.Data.DbContexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<IdentityContext>(new DropCreateDatabaseIfModelChanges<IdentityContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetail>().HasRequired(t => t.User);
            modelBuilder.Ignore<UserCollectedGig>();
            modelBuilder.Ignore<Gig>();
            modelBuilder.Ignore<Order>();
            modelBuilder.Ignore<UserInboxMessage>();
            base.OnModelCreating(modelBuilder);
        }
    }
}