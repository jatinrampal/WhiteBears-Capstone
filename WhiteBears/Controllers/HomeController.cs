using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Session["username"] != null) {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (Authentication.VerifyCredentials(username, password))
            {
                if (!Authentication.VerifyIfEnabled(username)) {
                    ViewBag.Error = "Your account is disabled.  Please contact your administrator";
                    return View();
                }

                Session["username"] = username;

                if (Authentication.VerifyIfAdmin(username)) {
                    return RedirectToAction("Index", "AdminPage");
                }
                return RedirectToAction("Index", "Dashboard");
            }
           
           ViewBag.Error = "Username or Password Incorrect.";
           return View(); 
        }

        [HttpPost]
        public string Login(string username, string password)
        {
            if (Authentication.VerifyCredentials(username, password))
            {
                if (!Authentication.VerifyIfEnabled(username))
                {
                    ViewBag.Error = "Your account is disabled.  Please contact your administrator";
                    return "failed";
                }

                Session["username"] = username;

                if (Authentication.VerifyIfAdmin(username))
                {
                    return "success";
                }
                return "failed";
            }

            ViewBag.Error = "Username or Password Incorrect.";
            return "failed";
        }

        [HttpGet]
        public ActionResult LogOut() {
            Session["username"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}