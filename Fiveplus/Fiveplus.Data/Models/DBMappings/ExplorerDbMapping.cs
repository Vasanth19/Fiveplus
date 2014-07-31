using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fiveplus.Data.Models;

namespace Fiveplus.Data.DBMappings
{
    /// <summary>
    /// All Table and Relationship configuration follows here 
    /// </summary>

    internal class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {

            Property(g => g.Created).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


        }
    }
    internal class GigConfiguration : EntityTypeConfiguration<Gig>
    {
        public GigConfiguration()
        {

            Property(g => g.Created).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(g => g.RowVersion).IsConcurrencyToken();
            //Property(g => g.RowVersion)
            Property(g => g.Title).HasMaxLength(100).IsRequired();


        }
    }

    internal class MediaConfiguration : EntityTypeConfiguration<Media>
    {
        public MediaConfiguration()
        {

            Property(g => g.Created).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(g => g.RowVersion).IsConcurrencyToken();

        }
    }


   
}
