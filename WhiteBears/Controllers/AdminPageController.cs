using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class AdminPageController : Controller
    {
        // GET: AdminPage
        public ActionResult Index()
        {
            DatabaseHelper dh = new DatabaseHelper();
            
            if (Session["username"] != null)
            {
                if (Authentication.VerifyIfAdmin(Session["username"].ToString())) {
                    return View();
                }else {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UserSettings()
        {
            if (Session["username"] != null)
            {

                List<User> users = AdminGetUsers().ToList();

                //if (model.CurrentUser.Role == "Admin") {
                return View(users);
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProjectSettings()
        {
            if (Session["username"] != null)
            {

                List<Project> projects = AdminGetAllProject().ToList();

                //if (model.CurrentUser.Role == "Admin") {
                return View(projects);
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProjectTable(int projectId)
        {
            Project project = AdminGetProject(projectId);
            return PartialView("ProjectTable", project);
        }

        public ActionResult TaskTable(int projectId)
        {
            Project[] projects = AdminGetAllProject();

            foreach(Project p in projects)
            {
                if(p.ProjectId == projectId){
                    return View(p.Tasks);
                }
            }
            return RedirectToAction("Index", "AdminPage");
        }

        public ActionResult DeleteProject(int projectId)
        {
            return Json(new { success = AdminDeleteProject(projectId) });
        }

        public ActionResult AddUser(string firstName, string lastName, string username, string email, string password, string role)
        {
            User newUser = new User(firstName, lastName, username, email, password, role)
            {
                CompanyId = 1,
                Enabled = true
            };
            return Json(new { success = AdminAddUser(newUser) });
        }

        public ActionResult ModifyUserFirstName(string username, string newInfo)
        {
            return Json(new { success = AdminChangeFirstName(username, newInfo) });
        }

        public ActionResult ModifyUserLastName(string username, string newInfo)
        {
            return Json(new { success = AdminChangeLastName(username, newInfo) });

        }

        public ActionResult ModifyUserPassword(string username, string newInfo)
        {
            return Json(new { success = AdminChangePassword(username, newInfo) });

        }

        public ActionResult ModifyUserUsername(string username, string newInfo)
        {
            return Json(new { success = AdminChangeUserName(username, newInfo) });
        }

        public ActionResult DisableUser(string username)
        {
            return Json(new { success = AdminDisableUser(username) });
        }

        public ActionResult EnableUser(string username)
        {
            return Json(new { success = AdminEnableUser(username) });
        }

        public static bool AdminDisableUser(string username)
        {
            // Marks Users are Disabled
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET [enabled] = 0 where [User].uName = '{username}';");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool AdminEnableUser(string username)
        {
            // Marks Users are Disabled
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET [enabled] = 1 where [User].uName = '{username}';");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Method to get user information.Parameter: string username.Returns a User object.
        // Returns Single User object 
        // Create getters and setters for password
        // Create Enabled field in User model + Enabled getters and setters 
        // Create CompanyId field in User model + CompanyId getters and setters 
        public static User AdminGetUser(string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{username}';");

                return new User()
                {
                    Username = drs[0]["uName"].ToString(),
                    CompanyId = Convert.ToInt32(drs[0]["companyId"]),
                    Password = drs[0]["password"].ToString(),
                    FirstName = drs[0]["firstName"].ToString(),
                    LastName = drs[0]["lastName"].ToString(),
                    Role = drs[0]["role"].ToString(),
                    Enabled = Convert.ToBoolean(drs[0]["enabled".ToString()])

                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // Empty User returned if exception is caught. 
                return new User()
                {
                    Username = "Null",
                    CompanyId = -1,
                    Password = "Null",
                    FirstName = "Null",
                    LastName = "Null",
                    Role = "Null",
                    Enabled = false
                };
            }
        }

        public static User[] AdminGetUsers()
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User];");
                User[] users = new User[drs.Count()];

                int i = 0;
                foreach(DataRow dr in drs){
                    users[i++] = new User()
                    {
                        Username = dr["uName"].ToString(),
                        CompanyId = Convert.ToInt32(dr["companyId"]),
                        Password = dr["password"].ToString(),
                        FirstName = dr["firstName"].ToString(),
                        LastName = dr["lastName"].ToString(),
                        Role = dr["role"].ToString(),
                        Enabled = Convert.ToBoolean(dr["enabled".ToString()])
                    };
                }

                return users;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static bool AdminDeleteProject(int projectId)
        {
            // Currently, Deletes only project from Project Table, but have included the the Queries to delete any Task information 
            // related to the project. 

            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"DELETE FROM Project WHERE projectId = '{projectId}';");

                // Comment IN the line below This will delete any Tasks assigned to that project. 
                //dh.RunQuery($"DELETE FROM User_Task WHERE projectId = '{projectId}';");
                //dh.RunQuery($"DELETE FROM Task WHERE projectId = '{projectId}';");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        //Method to add a new user to the database.Parameter: user object.  Returns a bool, if it was successful.
        public static bool AdminAddUser(User user)
        {
            // Adds new User to DB
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                string UserName = user.Username;
                int CompanyId = user.CompanyId;
                string Password = user.Password;
                string FirstName = user.FirstName;
                string LastName = user.LastName;
                string Role = user.Role;
                bool Enabled = user.Enabled;

                dh.RunQuery($"INSERT INTO [User] (uName, companyId, [password], firstName, lastName, [role], [enabled]) VALUES('{UserName}', '{CompanyId}', '{Password}', '{FirstName}', '{LastName}', '{Role}', '{Enabled}');");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        //Method to modify user: first name change.Parameter: string username & firstName.Returns a bool, if it was successful
        public static bool AdminChangeFirstName(string userName, string firstName)
        {
            // Changing firstName 
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET firstName = '{firstName}' WHERE [User].uName = '{userName}';");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        //Method to modify user: last name change.Parameter: string username & lastName.Returns a bool, if it was successful
        public static bool AdminChangeLastName(string userName, string lastName)
        {
            // Changing lastName 
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET lastName = '{lastName}' WHERE [User].uName = '{userName}';");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // Method to modify user: password change.Parameter: string username & password.Returns a bool, if it was successful
        public static bool AdminChangePassword(string userName, string password)
        {
            // Changing Password
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET password = '{password}' WHERE [User].uName = '{userName}';");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // Method to modify user: username change.Parameter: string currentUsername & string newUsername.  Returns a bool, if it was successful
        public static bool AdminChangeUserName(string currentUserName, string newUserName)
        {
            // Changing UserName
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunQuery($"UPDATE [User] SET uName = '{newUserName}' WHERE [User].uName = '{currentUserName}';");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // Error 
        public static Project[] AdminGetAllProject()
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM Project");

                Project[] p = new Project[drs.Count()];
                for (int i = 0; i < drs.Count(); i++)
                {
                    Project pt = new Project();
                    var projectIdSav = Convert.ToInt32(drs[i]["projectId"]);
                    pt.ProjectId = Convert.ToInt32(drs[i]["projectId"]);
                    pt.Title = drs[i]["title"].ToString();
                    pt.Description = drs[i]["description"].ToString();
                    pt.ScopeStatement = drs[i]["scopeStatement"].ToString();
                    pt.StartDate = Convert.ToDateTime(drs[i]["startDate"].ToString());
                    pt.DueDate = Convert.ToDateTime(drs[i]["dueDate"].ToString());
                    pt.CompletionDate = Convert.ToDateTime(drs[i]["completionDate"].ToString());


                    DataRow[] drs1 = dh.RunQuery($"select * from Task t where t.projectId = {projectIdSav};");
                    Task[] tk = new Task[drs1.Count()];
                    for (int a = 0; a < drs1.Count(); a++)
                    {
                        Task t = new Task();
                        t.TaskId = Convert.ToInt32(drs1[a]["taskid"]);
                        t.Title = drs1[a]["title"].ToString();
                        t.Description = drs1[a]["description"].ToString();
                        t.StartDate = Convert.ToDateTime(drs1[a]["startDate"].ToString());
                        t.DueDate = Convert.ToDateTime(drs1[a]["dueDate"].ToString());
                        t.CompletedDate = Convert.ToDateTime(drs1[a]["completionDate"].ToString());
                        t.ProjectId = Convert.ToInt32(drs1[a]["projectId"]).ToString();
                        t.Priority = drs1[a]["priority"].ToString();

                        tk[a] = t;
                    }
                    // Add Task[] to Project 
                    pt.Tasks = tk;
                    // Add Project to Project[]
                    p[i] = pt;
                }
                return p;
            }
            catch (Exception e)
            {
                Project[] p = new Project[0];
                Console.WriteLine("Failed");
                return p;
            }
        }

        public static Project AdminGetProject(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM Project WHERE projectId='{projectId}'");

                Project pt = new Project();
                pt.ProjectId = Convert.ToInt32(drs[0]["projectId"]);
                pt.Title = drs[0]["title"].ToString();
                pt.Description = drs[0]["description"].ToString();
                pt.ScopeStatement = drs[0]["scopeStatement"].ToString();
                pt.StartDate = Convert.ToDateTime(drs[0]["startDate"].ToString());
                pt.DueDate = Convert.ToDateTime(drs[0]["dueDate"].ToString());
                pt.CompletionDate = Convert.ToDateTime(drs[0]["completionDate"].ToString());


                DataRow[] drs1 = dh.RunQuery($"select * from Task t where t.projectId = '{projectId}';");
                Task[] tk = new Task[drs1.Count()];
                for (int a = 0; a < drs1.Count(); a++)
                {
                    Task t = new Task();
                    t.TaskId = Convert.ToInt32(drs1[a]["taskid"]);
                    t.Title = drs1[a]["title"].ToString();
                    t.Description = drs1[a]["description"].ToString();
                    t.StartDate = Convert.ToDateTime(drs1[a]["startDate"].ToString());
                    t.DueDate = Convert.ToDateTime(drs1[a]["dueDate"].ToString());
                    t.CompletedDate = Convert.ToDateTime(drs1[a]["completionDate"].ToString());
                    t.ProjectId = Convert.ToInt32(drs1[a]["projectId"]).ToString();
                    t.Priority = drs1[a]["priority"].ToString();

                    tk[a] = t;
                }
                // Add Task[] to Project 
                pt.Tasks = tk;

                return pt;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed");
                return null;
            }
        }

        // Method to check if username already exists.Parameter: string username.Returns a bool, if username doesn't exist
        public static bool userUserNameExists(string userName)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{userName}';");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}