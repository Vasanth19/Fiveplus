using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DBMappings
{
    /// <summary>
    /// All Table and Relationship configuration follows here 
    /// </summary>

   
    internal class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {

            HasRequired(o => o.CreatedUser).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).WillCascadeOnDelete(true);
            HasRequired(o => o.Gig).WithMany(g => g.Orders).HasForeignKey(o => o.GigId).WillCascadeOnDelete(true);
        }
    }
}
