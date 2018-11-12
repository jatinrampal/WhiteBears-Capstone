using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class TeamManagementController : Controller
    {
        public ActionResult Index(int? id)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string username = Session["username"].ToString();

            int projectId = id ?? default(int);


            Debug.WriteLine(projectId);

            TeamManagementModel model = new TeamManagementModel();
            model.CurrentProject = model.GetProject(projectId);
            model.ExcludedUsers = model.GetExcludedUsers(projectId);
            model.IncludedUsers = model.GetIncludedUsers(projectId);
            model.Projects = model.GetProjects(username);
            model.CurrentUser = model.GetUser(username);
            if (model.CurrentUser.Role != "Project Manager"){
                return RedirectToAction("Index", "Project", new { id = projectId });
            }

            return View(model);
        }

        public ActionResult AddUsers(string[] usernames, string[] includedUsers, int projectId)
        {
            List<string> toAdd = new List<string>();

            foreach (string s in usernames)
            {
                if (!includedUsers.Contains(s))
                {
                    toAdd.Add(s);
                }
            }

            TeamManagementModel.AddUserToProject(toAdd.ToArray(), projectId);
            return Json(new { success = true });
        }

        public ActionResult RemoveUsers(string[] usernames, string[] excludedUsers, int projectId)
        {
            List<string> toRemove = new List<string>();

            foreach (string s in usernames)
            {
                if (!excludedUsers.Contains(s))
                {
                    toRemove.Add(s);
                }
            }

            TeamManagementModel.RemoveUserFromProject(toRemove.ToArray(), projectId);
            return Json(new { success = true });
        }
    }
}