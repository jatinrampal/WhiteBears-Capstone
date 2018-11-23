using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;
using System.Data;

namespace WhiteBears.Controllers {
    public class AddProjectController : Controller {
        // GET: AddProject
        public ActionResult Index() {
            if (Session["username"] == null) {
                return RedirectToAction("Index", "Home");
            }
            string userName = Session["username"].ToString();
            DatabaseHelper db = new DatabaseHelper();
            DataRow[] dr = db.RunSelectQuery($"SELECT role FROM [user] WHERE uname = '{userName}'");
            if (!dr[0]["role"].ToString().Equals("Project Manager")) {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Go(string title, string description, string scope, DateTime start, DateTime due) {
            AddProject project = new AddProject() {
                Title = title,
                Description = description,
                ScopeStatement = scope,
                StartDate = start,
                DueDate = due
            };

            if (Session["username"] == null) {
                return RedirectToAction("Index", "Home");
            }
            if (project.Title == null || project.Description == null || project.ScopeStatement == null) {
                return View(project);
            }
            string userName = Session["username"].ToString();
            DatabaseHelper db = new DatabaseHelper();
            DataRow[] dr = db.RunSelectQuery($"SELECT role FROM [user] WHERE uname = '{userName}'");
            if (!dr[0]["role"].ToString().Equals("Project Manager")) {
                return RedirectToAction("Index", "Dashboard");
            }

            db.RunInsertQuery($"INSERT INTO project (title, description, scopestatement, startdate, duedate, completionDate) VALUES ('{project.Title}', '{project.Description}','{project.ScopeStatement}','{project.StartDate}','{project.DueDate}', '01/01/0001')");
            DataRow[] dr2 = db.RunSelectQuery($"SELECT projectid FROM project WHERE title = '{project.Title}'");
            db.RunInsertQuery($"INSERT INTO user_project (uname, projectid) VALUES('{userName}','{dr2[0]["projectid"]}')");


            return Json(new { success = true, id = dr2[0]["projectid"] });
        }
    }
}