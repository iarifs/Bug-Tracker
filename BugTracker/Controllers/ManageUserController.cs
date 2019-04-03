using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageUserController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private RoleManager<IdentityRole> roleManager;

        public ManageUserController()
        {
            roleManager = new RoleManager<IdentityRole> (new RoleStore<IdentityRole>(db));

        }

        // GET: ManageUser
        public ActionResult Index()
        {
            var users = db.Users.Select(p => new UserListViewModel()
            {
                Id = p.Id,
                UserName = p.UserName,
                ScreenName = p.ScreenName,

            }).ToList();

            var roleHelper = new UserRolesHelper(db);

            users.ForEach(p => p.Roles = roleHelper.ListUserRoles(p.Id));

            ViewBag.roles = roleManager.Roles
                .Select(p => p.Name).ToList();

            return View(users);
        }

        public ActionResult ChangeUserRole(string id)
        {
            return View();
        }
    }
}