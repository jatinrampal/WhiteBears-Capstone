using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            Project project1 = new Project
            {
                Title = "College",
                Tasks = new Task[2]
            };

            Project project2 = new Project
            {
                Title = "Life",
                Tasks = new Task[1]
            };

            project1.Tasks = new Task[] {
                new Task
                {
                    Title = "Complete Capstone",
                    Priority = 4,
                    DueDate = Convert.ToDateTime("09/23/2018"),
                    Status = "Behind",
                },

                new Task
                {
                    Title = "Graduate",
                    Priority = 5,
                    DueDate = Convert.ToDateTime("12/15/2018"),
                    Status = "On Time",
                }
            };

            project2.Tasks = new Task[] {
                new Task
                {
                    Title = "Get a job that pays $1,000,000 with lots of benefits and pension",
                    Priority = 100,
                    DueDate = Convert.ToDateTime("01/01/2019"),
                    Status = "On time",
                },
            };


            DashboardModel model = new DashboardModel
            {
                Projects = new Project[]
                {
                    project1, project2
                }

            };

            return View(model);
        }
    }
}