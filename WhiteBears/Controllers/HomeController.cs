using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {

            if (VerifyCredentials.VerifyLogin(username, password))
            {
                return View("UserDashboard");
            }
           
           ViewBag.Error = "Username or Password Incorrect.";
           return View();
            
            

        }
      
    }
}