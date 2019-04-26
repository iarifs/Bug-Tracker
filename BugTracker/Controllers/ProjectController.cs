using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Project Manager")]
    public class ProjectController : Controller
    {
        ApplicationDbContext Db = new ApplicationDbContext();
        UserRolesHelper rolehelper;

        public ProjectController()
        {
            rolehelper = new UserRolesHelper(Db);
        }

        public ActionResult Index()
        {
            var projectList = Db
                .Projects
                .Where(p => !p.IsArchived)
                .Select(n => new ProjectListViewModel
            {
                Id = n.Id,
                Name = n.Name,
                Description = n.Description,
                DateCreated = n.DateCreated,
                DateUpdated = n.DateUpdated,
                NumbersOfUsers = n.Users.Count,
                NumbersOfTickets = n.Tickets.Count,

            }).ToList();


            return View(projectList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateProjectViewModel form)
        {
            return ProjectCreator(null, form);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }

            var project = Db.Projects.Where(p => !p.IsArchived).FirstOrDefault(p => p.Id == id);

            var model = new CreateProjectViewModel
            {
                Name = project.Name,
                Description = project.Description,
            };

            if (project == null)
            {
                return RedirectToAction(nameof(ProjectController.Index));
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateProjectViewModel form)
        {
            return ProjectCreator(id, form);
        }

        [HttpGet]
        public ActionResult ChangeMember(int id)
        {
            var project = Db.Projects.Where(p => !p.IsArchived).FirstOrDefault(p => p.Id == id);

            var Allusers = Db.Users.Select(n => new UserListViewModel
            {
                Id = n.Id,
                ScreenName = n.ScreenName,
                Roles = (from userRoles in n.Roles
                         join roles in Db.Roles on userRoles.RoleId equals roles.Id
                         select roles.Name).ToList(),
            }).ToList();

            project.Users.ForEach(p => p.AssignRoles = rolehelper.ListUserRoles(p.Id).ToList());

            ViewBag.Allusers = Allusers;

            return View(project);
        }
        [HttpPost]
        public ActionResult ChangeMember(string userId, int projectId)
        {
            var project = Db.Projects.Where(p => !p.IsArchived).FirstOrDefault(n => n.Id == projectId);

            var findUser = Db.Users.FirstOrDefault(n => n.Id == userId);

            project.Users.Add(findUser);

            Db.SaveChanges();

            return RedirectToAction(nameof(ProjectController.ChangeMember));
        }

        [HttpPost]
        public ActionResult RemoveMember(string userId, int projectId)
        {

            var project = Db.Projects.Where(p => !p.IsArchived).FirstOrDefault(n => n.Id == projectId);

            var findUser = Db.Users.FirstOrDefault(n => n.Id == userId);

            project.Users.Remove(findUser);

            Db.SaveChanges();

            return RedirectToAction("ChangeMember", "Project", new { id = projectId });
        }

        public ActionResult ProjectCreator(int? id, CreateProjectViewModel form)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project();

                if (id.HasValue)
                {
                    project = Db.Projects.Where(p => !p.IsArchived).FirstOrDefault(p => p.Id == id);
                    if (project == null)
                    {
                        return RedirectToAction(nameof(ProjectController.Index));
                    }
                    project.DateUpdated = DateTime.Now;
                }
                else
                {
                    Db.Projects.Add(project);
                }

                project.Name = form.Name;
                project.Description = form.Description;
            }
            Db.SaveChanges();
            return RedirectToAction(nameof(ProjectController.Index));

        }
        [HttpGet]
        public ActionResult Archive()
        {
            var projectList = Db.
                Projects.
                Where(p => !p.IsArchived).
                Select(n => new ProjectListViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                }).ToList();

            return View(projectList);
        }
        [HttpPost]
        public ActionResult Archive(List<string> projectId)
        {
            var projectList = Db.Projects.ToList();

            if (projectId.Count() > 0)
            {
                foreach (var id in projectId)
                {
                    if (projectList.Any(p => p.Id.ToString() == id))
                    {
                        projectList.Find(p => p.Id.ToString() == id).IsArchived = true;
                    }
                }
            }
            Db.SaveChanges();

            return RedirectToAction(nameof(ProjectController.Archive));
        }

        [OverrideAuthorization]
        [Authorize(Roles = "Submitter,Developer,Admin,Project Manager")]
        public ActionResult MyProjects()
        {
            var userName = User.Identity.Name;
            var projectList = Db.Projects
                .Where(p => p.Users.Any(n => n.UserName == userName) && !p.IsArchived)
                .Select(n => new ProjectListViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    DateCreated = n.DateCreated,
                    DateUpdated = n.DateUpdated,
                    NumbersOfUsers = n.Users.Count,
                    NumbersOfTickets = n.Tickets.Count,
                }).ToList();


            return View(projectList);
        }
    }
}