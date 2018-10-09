using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class ProjectTasksController : Controller
    {
        
        // GET: ProjectTasks
        public ActionResult Index()
        {

            ProjectTaskModel taskModel = new ProjectTaskModel(); /// Needed as ProjectTaskModel is not static 
            IEnumerable<Task> td = taskModel.getTask("Kish", 1);
            //Task task = new Task();
            
            return View(td);
            //return PartialView(td);
        }
    }
}