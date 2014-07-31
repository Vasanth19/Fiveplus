using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DBMappings
{
    /// <summary>
    /// All Table and Relationship configuration follows here 
    /// </summary>


    internal class IdentityUserConfiguration : EntityTypeConfiguration<UserDetail>
    {
        public IdentityUserConfiguration()
        {

            HasRequired(t => t.User);

        }
    }

    internal class UserInboxConfiguration : EntityTypeConfiguration<UserInboxMessage>
    {
        public UserInboxConfiguration()
        {

            ToTable("UserInbox");

        }
    }

    internal class UserCollectedGigConfiguration : EntityTypeConfiguration<UserCollectedGig>
    {
        public UserCollectedGigConfiguration()
        {

            HasRequired(c => c.User).WithMany(u => u.CollectedGigs).HasForeignKey(c => c.UserId);
            HasRequired(c => c.Gig).WithMany(g => g.CollectedGigs).HasForeignKey(c => c.GigId);
        }
    }

   
}
