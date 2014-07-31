using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiveplus.Data.Repo;


namespace Fiveplus.Data.Models
{
    public enum RowStatus
    {
        Disabled = 0,
        Enabled = 1

    }
    public class Category : IBaseModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime? Created { get; set; }
        public RowStatus RowStatus { get; set; }
        
        [NotMapped]
        public State State { get; set; }

    }
    public class Gig : IBaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public RowStatus RowStatus { get; set; }
        public DateTime? Created { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public UserDetail User { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<Addon> AddonServices { get; set; }
        public ICollection<Media> MediaUrl { get; set; }
        public ICollection<UserCollectedGig> CollectedGigs { get; set; }
        public ICollection<Order> Orders { get; set; }

        [NotMapped]
        public State State { get; set; }


    }

    public class Addon : IBaseModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public DateTime? Created { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int GigId { get; set; }
        [ForeignKey("GigId")]
        public Gig Gig { get; set; }

        [NotMapped]
        public State State { get; set; }


    }

    public enum MediaType
    {
        Image = 1,
        Video
    }
    public class Media : IBaseModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public DateTime? Created { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int GigId { get; set; }
        [ForeignKey("GigId")]
        public Gig Gig { get; set; }

        [NotMapped]
        public State State { get; set; }

    }

}
