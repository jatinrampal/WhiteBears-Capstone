﻿using System;
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
                Session["username"] = username;
                return RedirectToAction("Index", "Dashboard");
            }
           
           ViewBag.Error = "Username or Password Incorrect.";
           return View();
            
            

        }
      
    }
}