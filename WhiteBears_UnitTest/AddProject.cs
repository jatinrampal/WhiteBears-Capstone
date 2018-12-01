using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteBears.Controllers;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using Microsoft.CSharp.RuntimeBinder;

namespace WhiteBears_UnitTest
{
    [TestClass]
    class AddProjectTest
    {
        [TestMethod]
        public void Add_New_Project()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            //Arrange
            AddProjectController hc = new AddProjectController();
            builder.InitializeController(hc);
            builder.Session["username"] = "Kalen";
            

            //Act
            ActionResult ar = hc.Go("testProject", "description", "scope", new DateTime(1999,12,12), new DateTime(2000, 12, 12));

            //Assert
            Assert.IsTrue(true);
        }
    }
}
