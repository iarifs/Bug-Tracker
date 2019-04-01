namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var userManager = new UserManager
                                    <ApplicationUser>
                                         (new UserStore
                                            <ApplicationUser>
                                                    (context));

            var roleManager = new RoleManager
                                    <IdentityRole>
                                        (new RoleStore
                                            <IdentityRole>
                                                (context));

            const string ADMIN = "Admin";

            ApplicationUser adminUser;

            CreateUser(ADMIN);

            //Function for seeding admin user.
            void CreateUser(string position)
            {
                if (!context.Roles.Any(p => p.Name == "Admin"))
                {
                    var adminRole = new IdentityRole(position);
                    roleManager.Create(adminRole);
                }

                if (!context.Users.Any(p => p.Email == ADMIN.ToLower() + "@mybugtracker.com"))
                {

                    adminUser = new ApplicationUser
                    {
                        UserName = ADMIN.ToLower() + "@mybugtracker.com",
                        Email = ADMIN.ToLower() + "@mybugtracker.com",
                    };

                    userManager.Create(adminUser, "Password-1");
                }

                else
                {
                    adminUser = context.Users.First(p => p.UserName == position.ToLower() + "@mybugtracker.com");
                }

                if (!userManager.IsInRole(adminUser.Id, position))
                {
                    userManager.AddToRole(adminUser.Id, position);
                }
            }
        }

    }
}
