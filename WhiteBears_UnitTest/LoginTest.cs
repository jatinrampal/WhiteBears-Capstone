using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteBears.Controllers;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using WhiteBears.Models;

namespace WhiteBears_UnitTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void Login_Successfully()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            //Arrange
            HomeController hc = new HomeController();
            builder.InitializeController(hc);
            string userName = "Kalen";
            string password = "admin";
            string expectedValue = "Dashboard";

            //Act
            RedirectToRouteResult ar = hc.Index(userName, password) as RedirectToRouteResult;
            string abc = ar.RouteValues.GetValue("controller").ToString();

            //Assert
            Assert.AreEqual(abc, expectedValue);
        }

        [TestMethod]
        public void Login_Unsuccessful_IncorrectPassword()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            //Arrange
            HomeController hc = new HomeController();
            builder.InitializeController(hc);
            string userName = "Kalen";
            //wrong password
            string password = "admi";
            string expectedValue = "Username or Password Incorrect.";

            //Act
            ViewResult ar = hc.Index(userName, password) as ViewResult;
            
            //Assert
            Assert.AreEqual(ar.ViewBag.Error, expectedValue);
        }

        [TestMethod]
        public void Add_New_Project()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            //Arrange
            AddProjectController hc = new AddProjectController();
            builder.InitializeController(hc);
            builder.Session["username"] = "Kalen";

            //Act
            JsonResult ar = hc.Go("testProject", "description", "scope", new DateTime(1999, 12, 12), new DateTime(2000, 12, 12)) as JsonResult;
            string result = ar.Data.ToString();
            result = result.Substring(12, 4);
           

            //Assert
            Assert.IsTrue(bool.Parse(result));
        }

        [TestMethod]
        public void Load_Project()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            //Arrange
            ProjectController hc = new ProjectController();
            builder.InitializeController(hc);
            builder.Session["username"] = "Kalen";
            string expectedValue = "The Apple Initiative ";

            //Act
            ViewResult ar = hc.Index(1) as ViewResult;
            ProjectPageViewModel pm = ar.Model as ProjectPageViewModel;
            
            //Assert
            Assert.AreEqual(pm.Project.Title, expectedValue);
        }



    }
}
