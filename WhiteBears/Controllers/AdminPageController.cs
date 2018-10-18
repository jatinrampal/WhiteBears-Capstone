using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers {
    public class AdminPageController : Controller {
        // GET: AdminPage
        public ActionResult Index() {
            if (Session["username"] != null) {

                //if (model.CurrentUser.Role == "Admin") {
                return View();
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            } else {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UserSettings() {
            if(Session["username"] != null) {

                List<User> users = new List<User> {
                    new User("Kalen", "Rose", "Kalen", "Admin4", "PM"),
                    new User("Jatin", "Rampal", "Jatin", "Admin4", "Wopm"),
                    new User("Kish", "Dalal", "Kish", "Admin2", "Asas"),
                    new User("Rafael", "Sigwalt", "Raf", "Admin1", "Esdasd"),
                    new User("Mark", "Orlando", "Mark", "Admin100", "Teacher")
                };

                //if (model.CurrentUser.Role == "Admin") {
                return View(users);
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            } else {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProjectSettings() {
            if (Session["username"] != null) {

                Task task1 = new Task() {
                    TaskId = 1,
                    Title = "Task1",
                    Description = "Description1",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CompletedDate = DateTime.Now,
                    Priority = "High"
                };

                Task task2 = new Task() {
                    TaskId = 2,
                    Title = "Task2",
                    Description = "Description2",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CompletedDate = DateTime.Now,
                    Priority = "High"
                };

                Task task3 = new Task() {
                    TaskId = 3,
                    Title = "Task3",
                    Description = "Description3",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CompletedDate = DateTime.Now,
                    Priority = "High"
                };

                Task task4 = new Task() {
                    TaskId = 4,
                    Title = "Task4",
                    Description = "Description4",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CompletedDate = DateTime.Now,
                    Priority = "High"
                };

                Task task5 = new Task() {
                    TaskId = 5,
                    Title = "Task5",
                    Description = "Description5",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CompletedDate = DateTime.Now,
                    Priority = "High"
                };

                List<Project> projects = new List<Project> {
                    new Project() {
                        ProjectId = 1,
                        Title = "Project 1",
                        Tasks = new Task[]{task1, task2},
                        Description = "Proj Desc 1",
                        ScopeStatement = "Proj Scope 1",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    },

                    new Project() {
                        ProjectId = 2,
                        Title = "Project 2",
                        Tasks = new Task[]{task3, task4, task5},
                        Description = "Proj Desc 2",
                        ScopeStatement = "Proj Scope 2",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    }
                };

                //if (model.CurrentUser.Role == "Admin") {
                return View(projects);
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            } else {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProjectTable(int projectId) {
            Task task1 = new Task() {
                TaskId = 1,
                Title = "Task1",
                Description = "Description1",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task2 = new Task() {
                TaskId = 2,
                Title = "Task2",
                Description = "Description2",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task3 = new Task() {
                TaskId = 3,
                Title = "Task3",
                Description = "Description3",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task4 = new Task() {
                TaskId = 4,
                Title = "Task4",
                Description = "Description4",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task5 = new Task() {
                TaskId = 5,
                Title = "Task5",
                Description = "Description5",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            List<Project> projects = new List<Project> {
                    new Project() {
                        ProjectId = 1,
                        Title = "Project 1",
                        Tasks = new Task[]{task1, task2},
                        Description = "Proj Desc 1",
                        ScopeStatement = "Proj Scope 1",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    },

                    new Project() {
                        ProjectId = 2,
                        Title = "Project 2",
                        Tasks = new Task[]{task3, task4, task5},
                        Description = "Proj Desc 2",
                        ScopeStatement = "Proj Scope 2",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    }
                };

            Project p = null;

            foreach(Project projects1 in projects) {
                if (projects1.ProjectId == projectId)
                    p = projects1;
            }

            return PartialView("ProjectTable", p);
        }

        public ActionResult TaskTable(int projectId) {
            Task task1 = new Task() {
                TaskId = 1,
                Title = "Task1",
                Description = "Description1",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task2 = new Task() {
                TaskId = 2,
                Title = "Task2",
                Description = "Description2",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task3 = new Task() {
                TaskId = 3,
                Title = "Task3",
                Description = "Description3",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task4 = new Task() {
                TaskId = 4,
                Title = "Task4",
                Description = "Description4",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            Task task5 = new Task() {
                TaskId = 5,
                Title = "Task5",
                Description = "Description5",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now,
                CompletedDate = DateTime.Now,
                Priority = "High"
            };

            List<Project> projects = new List<Project> {
                    new Project() {
                        ProjectId = 1,
                        Title = "Project 1",
                        Tasks = new Task[]{task1, task2},
                        Description = "Proj Desc 1",
                        ScopeStatement = "Proj Scope 1",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    },

                    new Project() {
                        ProjectId = 2,
                        Title = "Project 2",
                        Tasks = new Task[]{task3, task4, task5},
                        Description = "Proj Desc 2",
                        ScopeStatement = "Proj Scope 2",
                        StartDate = DateTime.Now,
                        DueDate = DateTime.Now,
                        CompletionDate = DateTime.Now,
                    }
                };

            Task[] tasks1 = null;

            foreach (Project projects1 in projects) {
                if (projects1.ProjectId == projectId)
                    tasks1 = projects1.Tasks;
            }
            return View(tasks1);
        }

        public ActionResult DeleteProject(int projectId) {
            //Database code here

            return null;
        }

        public ActionResult AddUser(string firstName, string lastName, string username, string password, string role) {
            User newUser = new User(firstName, lastName, username, password, role);

            //Database code here

            return null;
        }

        public ActionResult ModifyUserFirstName(string username, string newInfo) {
            //Database code here

            return null;
        }

        public ActionResult ModifyUserLastName(string username, string newInfo) {
            //Database code here

            return null;
        }

        public ActionResult ModifyUserPassword(string username, string newInfo) {
            //Database code here

            return null;
        }

        public ActionResult ModifyUserUsername(string username, string newInfo) {
            //Database code here

            return null;
        }
    }
}