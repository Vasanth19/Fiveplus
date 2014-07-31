using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiveplus.Data.Models
{
   public enum OrderStatus
    {
        Active = 1,
        Completed,
        AwaitingReview,
        Delivered,
        MissingDetails,
        Cancelled

        
    }
    public class Order
    {
        public int Id { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
        public DateTime Created { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }

        //FK for gig
        public int GigId { get; set; }
        [ForeignKey("GigId")]
        public Gig Gig { get; set; }

        //Foreign Key for user
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDetail CreatedUser { get; set; }
    }


    /// <summary>
    /// LineItems would typically consists of AddOn services opted.
    /// </summary>
    public class LineItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price{ get; set; }
        public DateTime Created { get; set; }
      
    }
}
