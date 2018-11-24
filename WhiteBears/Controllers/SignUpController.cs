using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers {
    public class SignUpController : Controller {
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUsername(string username) {
            DatabaseHelper dh = new DatabaseHelper();
            if (dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{username}'").Count() == 1) {
                return Json(new { success = false });
            } else {
                return Json(new { success = true });
            }
        }

        public ActionResult Go(string firstname, string lastname, string username, string email, string password, string companyname, DateTime companyformed, string title, string description, string scope, string startDate, string dueDate) {
            User u = new User(firstname, lastname, username, email, password, "Admin");
            Company c = new Company(companyname, companyformed);

            DatabaseHelper dh = new DatabaseHelper();

            dh.RunInsertQuery($"INSERT INTO Company VALUES('{c.Name}')");
            dh.RunInsertQuery($"INSERT INTO Project VALUES('{title}', '{description}', '{scope}', '{startDate}', '{dueDate}', '01/01/0001')");

            DataRow[] drs = dh.RunQuery($"SELECT MAX(companyId) FROM Company");

            string companyId = drs[0][0].ToString();

            dh.RunInsertQuery($"INSERT INTO [User] VALUES('{u.Username}', '{companyId}', '{u.Password}', '{u.FirstName}', '{u.LastName}', '{u.Role}', 'true')");

            return Json(new { success = true });
        }
    }
}