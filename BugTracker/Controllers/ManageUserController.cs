using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private RoleManager<IdentityRole> roleManager;

        private UserManager<ApplicationUser> userManager;

        private UserRolesHelper roleHelper;

        public ManageUserController()
        {
            roleManager = new RoleManager<IdentityRole>
                            (new RoleStore<IdentityRole>(db));
            userManager = new UserManager<ApplicationUser>
                            (new UserStore<ApplicationUser>
                                (new ApplicationDbContext()));
            roleHelper = new UserRolesHelper(db);

        }

        // GET: ManageUser
        public ActionResult Index()
        {
            var users = db.Users.Select(p => new UserListViewModel()
            {
                Id = p.Id,
                UserName = p.UserName,
                ScreenName = p.ScreenName,
                Roles = (from userRoles in p.Roles
                         join roles in db.Roles on userRoles.RoleId equals roles.Id
                         select roles.Name).ToList(),
            }).ToList();

            ViewBag.AllRolesInApp = roleManager.Roles.Select(p => p.Name).ToList();

            return View(users);
        }

        [HttpGet]
        public ActionResult ChangeUserRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View("Error");
            }

            if (userManager.FindById(id) == null)
            {
                return View("Error");
            }

            var model = db.Users
                .Where(p => p.Id == id)
                .Select(n => new UserListViewModel()
                {
                    Id = n.Id,
                    UserName = n.UserName,
                    ScreenName = n.ScreenName,
                }).FirstOrDefault();

            model.Roles = roleHelper.ListUserRoles(id).ToList();

            ViewBag.AllRolesInApp = roleManager.Roles.Select(p => p.Name).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeUserRole(string id, List<string> Roles)
        {
            var userRoles = userManager.GetRoles(id);

            var user = userManager.FindById(id);

            if (Roles == null)
            {
                userManager.RemoveFromRoles(id, userRoles.ToArray());
            }
            else
            {
                //first remove all the roles and then add roles that are passing
                userManager.RemoveFromRoles(id, userRoles.ToArray());

                userManager.AddToRoles(id, Roles.ToArray());
            }

            return RedirectToAction(nameof(ManageController.Index));
        }
    }
}