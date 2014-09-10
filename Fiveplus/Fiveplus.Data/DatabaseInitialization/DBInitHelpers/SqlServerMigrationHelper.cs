using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fiveplus.Data.DatabaseInitialization
{
    static class SqlServerMigrationHelper
    {
        internal static void SeedAppData(Fiveplus.Data.DbContexts.FiveplusContext context)
        {
            //  This method will be called after migrating to the latest version.


            CreateCategories(context);

            ApplicationUser user = CreateRoleandUser(context);
            CreateUserdetails(context, user);
            CreateGigs(context, user);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }
            
        }

        private static ApplicationUser CreateRoleandUser(FiveplusContext context)
        {
            #region  Initial Code

            /*
            string roleName = "Administrator";
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists(roleName))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = roleName;
                roleManager.Create(role);
            }  

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            const string name = "admin@example.com";
            const string password = "Pass@word1";

            var user = new ApplicationUser { UserName = name, Email = name };
            var result = userManager.Create(user,password);

            if (result.Succeeded)
                userManager.AddToRole(user.Id, roleName);

           return user;
             */
            #endregion

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            const string name = "admin";
            const string email = "admin@fiveplus.com";
            const string password = "Pass@word1";
            const string roleName = "Administrator";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null) {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null) {
                user = new ApplicationUser { UserName = name, Email = email };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name)) {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            return user;
        }

        private static void CreateUserdetails(FiveplusContext context, ApplicationUser user)
        {

            context.UserDetails.AddOrUpdate(u => u.Id,
                new UserDetail
                {
                    FullName = "World Explorer",
                    Biography = "Travel the wold withour hesitation",
                    Preference = NotificationPreference.Weekly,
                    Timezone = "Eastern",
                    ProfileImg = @"http://www.gravatar.com/avatar.php?gravatar_id=97247f5541474a5f62e02cef888b738d&rating=PG&size=110&default=identicon",
                    User = user
                });

        }

        private static void CreateCategories(FiveplusContext context)
        {
            context.Categories.AddOrUpdate(c => c.CategoryName,
                 new Category { CategoryName = "Video & Animation", RowStatus = RowStatus.Enabled},
                 new Category { CategoryName = "Graphics & Design", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Writing & Transalation", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Music  & Audio", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Advertising", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Flat Rate Merchandise", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Business", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Programming and Tech", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Fun & Unique", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Business", RowStatus = RowStatus.Enabled },
                 new Category { CategoryName = "Others", RowStatus = RowStatus.Enabled }

                 );

        }

        private static void CreateGigs(FiveplusContext context, ApplicationUser user)
        {
            context.Gigs.AddOrUpdate(g => g.Id,
             new Gig
             {

                 Title = "I can write you a awesome Resume I can write you a awesome Resume",
                 Description = "I can write an awesome Resume to you in ay way i want I can write you a awesome ResumeI can write you a awesome ResumeI can write you a awesome ResumeI can write you a awesome ResumeI can write you a awesome ResumeI can write you a awesome Resume",
                 Price = 4.50,
                 UserId = user.Id,
                 RowStatus = RowStatus.Enabled,
                 CategoryId = 3,
                 AddonServices = new Collection<Addon>()
                 {
                     new Addon() { Description = "Add a Formating Service", Price = 3.99},
                     new Addon() { Description = "Add a Double Formating Service", Price = 3.99}
                 },
                 MediaUrls = new Collection<Media>()
                 {
                     new Media(){Url = @"http://localhost:59903/assets/img/job/high-rated-job-1.1.jpg",Type = MediaType.Image},
                     new Media(){Url = @"http://localhost:59903/assets/img/job/high-rated-job-1.2.jpg",Type = MediaType.Image},
                     new Media(){Url = @"http://localhost:59903/assets/img/job/high-rated-job-1.3.jpg",Type = MediaType.Image}
                 }
             }
             );
        }
    }

}
