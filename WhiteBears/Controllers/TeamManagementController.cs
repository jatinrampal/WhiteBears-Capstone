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

            if(Session["username"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string username = Session["username"].ToString();

            int projectId = id ?? default(int);

            TeamManagementModel model = new TeamManagementModel();
            model.CurrentProject = model.GetProject(projectId);
            model.ExcludedUsers = model.GetExcludedUsers(projectId);
            model.IncludedUsers = model.GetIncludedUsers(projectId);
            model.Projects = model.GetProjects(username);
            model.CurrentUser = model.GetUser(username);

            return View(model);
        }

        public ActionResult Update(string[] usernames, int projectId)
        {
            TeamManagementModel.AddUserToProject(usernames, projectId);
            return Json(new { success = true });
        }

        public ActionResult Cancel(){
            return RedirectToAction("Index", "Home");
        }
    }
}