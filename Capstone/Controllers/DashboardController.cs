using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears;

namespace Capstone.Controllers
{
    public class DashboardController : Controller
    {

        Project[] projects;
        User currUser;


        private readonly SqlConnection conn;
        public static SqlDataAdapter da;
        private DataSet ds;

        public ActionResult Index()
        {
            DataRow[] drs, drs1;

            DashboardModel model = new DashboardModel();

            drs = model.GetUser("Kalen");
            drs1 = model.GetUserRole("Kalen");

            currUser = new User(drs[0]["firstName"].ToString(),
                drs[0]["lastName"].ToString(),
                drs[0]["uName"].ToString(),
                drs[0]["password"].ToString(),
                drs1[0]["role"].ToString());


            drs = model.GetProjects(currUser.Username);
            projects = new Project[drs.Count()];

            int i = 0;
            foreach (DataRow dr in drs) {
                int currProjectId = Int32.Parse(drs[i]["projectId"].ToString());
                string projectTitle = dr["title"].ToString();
                drs1 = model.GetTasks(currUser.Username, currProjectId);

                Task[] currProjectTasks = new Task[drs1.Count()];
                int j = 0;
                foreach (DataRow dr1 in drs1) {
                    DateTime dueDate = Convert.ToDateTime(dr1["dueDate"]);
                    currProjectTasks[j++] = new Task
                    {
                        Title = dr1["title"].ToString(),
                        Priority = 0,
                        DueDate = dueDate,
                        Status = DateTime.Now < dueDate ? "On time" : "Overdue",
                        ProjectName = projectTitle
                    };
                }

                projects[i++] = new Project
                {
                    ProjectId = Int32.Parse(dr["projectId"].ToString()),
                    Title = projectTitle,
                    Tasks = currProjectTasks
                };
            }

            currUser.PersonalNotes = GetPersonalNotes(model);

            model.Projects = projects;
            model.CurrentUser = currUser;
            model.CurrDate = DateTime.Now;

            return View(model);
        }

        private PersonalNote[] GetPersonalNotes(DashboardModel model){
            DataRow[] drs = model.GetPersonalNote(currUser.Username);
            PersonalNote[] notes = new PersonalNote[drs.Count()];
            int i = 0;
            foreach (DataRow dr in drs)
            {
                notes[i++] = new PersonalNote(Int32.Parse(dr["noteId"].ToString()), dr["note"].ToString(), Convert.ToDateTime(dr["date"]));
            }

            return notes;
        }

        [HttpPost]
        public ActionResult UpdateProject(string projectId)
        {
            DataRow[] drs, drs1;

            DashboardModel model = new DashboardModel();

            drs = model.GetUser("Kalen");
            drs1 = model.GetUserRole("Kalen");

            currUser = new User(drs[0]["firstName"].ToString(),
                drs[0]["lastName"].ToString(),
                drs[0]["uName"].ToString(),
                drs[0]["password"].ToString(),
                drs1[0]["role"].ToString());


            drs = model.GetProjects(currUser.Username);
            projects = new Project[drs.Count()];

            int i = 0;
            foreach (DataRow dr in drs)
            {
                int currProjectId = Int32.Parse(drs[i]["projectId"].ToString());
                string projectTitle = dr["title"].ToString();
                drs1 = model.GetTasks(currUser.Username, currProjectId);

                Task[] currProjectTasks = new Task[drs1.Count()];
                int j = 0;
                foreach (DataRow dr1 in drs1)
                {
                    DateTime dueDate = Convert.ToDateTime(dr1["dueDate"]);
                    currProjectTasks[j++] = new Task
                    {
                        Title = dr1["title"].ToString(),
                        Priority = 0,
                        DueDate = dueDate,
                        Status = DateTime.Now < dueDate ? "On time" : "Overdue",
                        ProjectName = projectTitle
                    };
                }

                projects[i++] = new Project
                {
                    ProjectId = Int32.Parse(dr["projectId"].ToString()),
                    Title = projectTitle,
                    Tasks = currProjectTasks
                };
            }

            currUser.PersonalNotes = GetPersonalNotes(model);

            model.Projects = projects;
            model.CurrentUser = currUser;
            model.CurrDate = DateTime.Now;

            return null;
        }
        

        [HttpPost]
        public ActionResult AddNote(string input, string username){
            DashboardModel model = new DashboardModel();

            string results = ""+model.AddPersonalNote(username, input);
            return Json(new { success = true, message = results });
        }

        [HttpPost]
        public ActionResult DeleteNote(string noteId)
        {
            DashboardModel model = new DashboardModel();

            string results = "" + model.DeletePersonalNote(Int32.Parse(noteId));
            return Json(new { success = true, message = noteId });
        }

        [HttpPost]
        public ActionResult UpdateNote(string input, string noteId)
        {
            DashboardModel model = new DashboardModel();

            string results = "" + model.UpdatePersonalNote(input, Int32.Parse(noteId));
            return Json(new { success = true, message = noteId });
        }
    }
}