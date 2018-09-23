using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class DashboardController : Controller
    {

        Project[] projects = new Project[2];
        User currUser;
        public ActionResult Index()
        {
            projects[0] = new Project
            {
                ProjectId = 1,
                Title = "College",
                Tasks = new Task[2]
            };

            projects[1] = new Project
            {
                ProjectId = 2,
                Title = "Life",
                Tasks = new Task[1]
            };

            projects[0].Tasks = new Task[] {
                new Task
                {
                    Title = "Complete Capstone",
                    Priority = 4,
                    DueDate = Convert.ToDateTime("09/23/2018"),
                    Status = "Behind",
                    ProjectName = projects[0].Title
                },

                new Task
                {
                    Title = "Graduate",
                    Priority = 5,
                    DueDate = Convert.ToDateTime("12/15/2018"),
                    Status = "On Time",
                    ProjectName = projects[0].Title
                }
            };

            projects[1].Tasks = new Task[] {
                new Task
                {
                    Title = "Get a job that pays $1,000,000 with lots of benefits and pension",
                    Priority = 100,
                    DueDate = Convert.ToDateTime("01/01/2019"),
                    Status = "On time",
                    ProjectName = projects[1].Title
                },
            };


            PersonalNote[] notes = new PersonalNote[]
            {
                new PersonalNote
                {
                    Information = "We don't have much time left.",
                    TimeStamp = Convert.ToDateTime("09/17/2018")
                },

                new PersonalNote
                {
                    Information = "Thankfully we will make 1,000,000 a year for the rest of our lives.",
                    TimeStamp = Convert.ToDateTime("09/18/2018")
                },

                new PersonalNote
                {
                    Information = "...will we?",
                    TimeStamp = Convert.ToDateTime("09/19/2018")
                }
            };

            currUser = new User("Kalen", "Rose")
            {
                PersonalNotes = notes
            };

            DashboardModel model = new DashboardModel
            {
                Projects = new Project[]
                {
                    projects[0], projects[1]
                },

                CurrentUser = currUser,
                CurrDate = DateTime.Now
            };

            return View(model);
        }
        
        public ActionResult UpdateProject(){
            Project selectedProject = projects[1];

            DashboardModel model = new DashboardModel
            {

                Projects = new Project[]
                {
                    selectedProject
                },

                CurrentUser = currUser,
                CurrDate = Convert.ToDateTime("01/01/0001")
            };
            
            return RedirectToAction("Index");
        }
    }
}