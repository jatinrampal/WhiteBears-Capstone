using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhiteBears.Controllers
{
    public class ProjectNotesController : Controller
    {
        // GET: ProjectNotes
        public ActionResult Index()
        {
            return View();
        }
    }
}