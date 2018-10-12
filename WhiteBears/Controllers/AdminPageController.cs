using System.Collections.Generic;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers {
    public class AdminPageController : Controller {
        // GET: AdminPage
        public ActionResult Index() {
            return View();
        }

        public ActionResult UserSettings() {
            if(Session["username"] != null) {

                List<User> users = new List<User>();

                users.Add(new User("Kalen", "Rose", "Kalen", "Admin4", "PM"));
                users.Add(new User("Jatin", "Rampal", "Jatin", "Admin4", "Wopm"));
                users.Add(new User("Kish", "Dalal", "Kish", "Admin2", "Asas"));
                users.Add(new User("Rafael", "Sigwalt", "Raf", "Admin1", "Esdasd"));
                users.Add(new User("Mark", "Orlando", "Mark", "Admin100", "Teacher"));

                //if (model.CurrentUser.Role == "Admin") {
                    return View(users);
                //} else {
                //    return RedirectToAction("Index", "Dashboard");
                //}
            } else {
                return RedirectToAction("Index", "Home");
            }
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