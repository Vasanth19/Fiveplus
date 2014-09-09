using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Fiveplus.Data.Repo;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fiveplus.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public UserDetail User { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

    }

    public class UserDetail : IBaseModel
    {
        [Key,Column("UserId")]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }

        public string FullName { get; set; }
        public string ProfileImg { get; set; }

        public string Location { get; set; }

        public string Timezone { get; set; }

        public string Biography { get; set; }

        public NotificationPreference Preference { get; set; }

        public ICollection<Gig> Gigs { get; set; }

        public ICollection<UserCollectedGig> CollectedGigs { get; set; }

        public ICollection<UserInboxMessage> UserInboxMessages { get; set; }

        public ICollection<Order> Orders { get; set; }

        public DateTime? Created { get; set; }

        [NotMapped]
        public State State { get; set; }


    }

    public enum NotificationPreference
    {
        Weekly = 1,
        TwiceWeekly,
        Infrequent
    }
    public class UserCollectedGig : IBaseModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public UserDetail User { get;  set; }

        public int GigId { get; set; }
        [ForeignKey("GigId")]
        public Gig Gig { get;  set; }


        public DateTime? Created { get; set; }

        [NotMapped]
        public State State { get; set; }
    }

    public class UserInboxMessage : IBaseModel
    {
        public int Id { get; set; }
        public UserDetail User { get; set; }

        public string Sender { get; set; }
        public string Message { get; set; }


        public DateTime? Created { get; set; }

        [NotMapped]
        public State State { get; set; }


    }
}