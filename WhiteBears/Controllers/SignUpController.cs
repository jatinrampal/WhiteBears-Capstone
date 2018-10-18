using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers {
    public class SignUpController : Controller {
        // GET: SignUp
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUsername(string username) {
            //Database code goes here

            return null;
        }

        [HttpPost]
        public ActionResult Go(string firstname, string lastname, string username, string password, string companyname, DateTime companyformed, string title, string description, string scope, DateTime startDate, DateTime dueDate) {
            User u = new User(firstname, lastname, username, password, "admin");
            Project p = new Project(title, description, scope, startDate, dueDate);
            Company c = new Company(companyname, companyformed);

            //Database code goes here
            return null;
        }
    }
}