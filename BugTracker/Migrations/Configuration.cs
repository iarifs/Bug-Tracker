namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using BugTracker.Models.Domain;
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
            const string MANAGER = "Project Manager";
            const string DEVELOPER = "Developer";
            const string SUBMITTER = "Submitter";
            const string DEMO = "Demo.";

            ApplicationUser adminUser;

            CreateRole(ADMIN);
            CreateRole(MANAGER);
            CreateRole(DEVELOPER);
            CreateRole(SUBMITTER);

            CreateUser(ADMIN);


            //creating demo roles
            CreateDemoUser(MANAGER);
            CreateDemoUser(DEVELOPER);
            CreateDemoUser(SUBMITTER);

            //seeding info related to Ticket

            SeedTicketTypes(nameof(TypesOfTickets.Bug));
            SeedTicketTypes(nameof(TypesOfTickets.Database));
            SeedTicketTypes(nameof(TypesOfTickets.Feature));
            SeedTicketTypes(nameof(TypesOfTickets.Support));

            SeedTicketStatus(nameof(TypesOfTicketStatuses.Open));
            SeedTicketStatus(nameof(TypesOfTicketStatuses.Resolved));
            SeedTicketStatus(nameof(TypesOfTicketStatuses.Rejected));


            SeedTicketPriorities(nameof(TypesOfTicektPriorites.High));
            SeedTicketPriorities(nameof(TypesOfTicektPriorites.Medium));
            SeedTicketPriorities(nameof(TypesOfTicektPriorites.Low));


            //Function for seeding admin user.
            void CreateUser(string user)
            {
                if (!context.Users.Any(p => p.Email == user.ToLower() + "@mybugtracker.com"))
                {

                    adminUser = new ApplicationUser
                    {
                        UserName = user.ToLower().Replace(" ", "-") + "@mybugtracker.com",
                        Email = user.ToLower().Replace(" ", "-") + "@mybugtracker.com",
                        EmailConfirmed = true,
                        ScreenName = "Admin",
                    };

                    userManager.Create(adminUser, "Password-1");
                }

                else
                {
                    adminUser = context.Users.First(p => p.UserName == user.ToLower() + "@mybugtracker.com");
                }

                if (!userManager.IsInRole(adminUser.Id, user))
                {
                    userManager.AddToRole(adminUser.Id, user);
                }
            }

            void CreateRole(string position)
            {
                if (!context.Roles.Any(p => p.Name == position))
                {
                    var Role = new IdentityRole(position);
                    roleManager.Create(Role);
                }
            }

            void CreateDemoUser(string user)
            {
                if (System.Diagnostics.Debugger.IsAttached == false)
                {
                    //! Uncomment line below to start debugging the Seed() method
                    System.Diagnostics.Debugger.Launch();
                }

                if (!context.Users.Any(p => p.Email == "demo." + user.ToLower() + "@mybugtracker.com"))
                {

                    adminUser = new ApplicationUser
                    {
                        UserName = "demo." + user.ToLower().Replace(" ", "-") + "@mybugtracker.com",
                        Email = "demo." + user.ToLower().Replace(" ","-") + "@mybugtracker.com",
                        EmailConfirmed = true,
                        ScreenName = "Demo-" + user,
                    };

                    userManager.Create(adminUser, "Password-1");
                }

                else
                {
                    adminUser = context.Users.First(p => p.UserName == "demo." + user.ToLower() + "@mybugtracker.com");
                }

                if (!userManager.IsInRole(adminUser.Id, user))
                {
                    userManager.AddToRole(adminUser.Id, user);
                }
            }


            void SeedTicketTypes(string name)
            {
                if (!context.TicketTypes.Any(p => p.Name == name))
                {
                    context.TicketTypes.Add(new TicketType { Name = name });
                }
                context.SaveChanges();
            }

            void SeedTicketStatus(string name)
            {
                if (!context.TicketStatuses.Any(p => p.Name == name))
                {
                    context.TicketStatuses.Add(new TicketStatus { Name = name });
                }
                context.SaveChanges();
            }

            void SeedTicketPriorities(string name)
            {
                if (!context.TicketPriorites.Any(p => p.Name == name))
                {
                    context.TicketPriorites.Add(new TicketPriority { Name = name });
                }
                context.SaveChanges();
            }

           
        }
    }
}