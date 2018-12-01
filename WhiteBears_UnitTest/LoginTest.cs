using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteBears.Controllers;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Microsoft.CSharp.RuntimeBinder;

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


    }
}
