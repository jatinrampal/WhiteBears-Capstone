using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhiteBears.Models;

namespace WhiteBears.Controllers
{
    public class ProjectController : Controller
    {

        // GET: Project
        public ActionResult Index(int? id)
        {

            //hey yo
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                string username = Session["username"].ToString();
                int projectid = Convert.ToInt32(id);

                ProjectPageModel p = new ProjectPageModel();
                var userExists = p.userAccessProject(projectid, username);
               
                if(userExists == false)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                /*
                ProjectModel pm = new ProjectModel();
                Project p = new Project();
                p = pm.GetProject(username, projectid);
                string Result = Request.Params["result"];
                ViewBag.Message = Result;
                */
                ProjectPageViewModel pm = new ProjectPageViewModel();
                
                
                
                if(p.GetProject(username, projectid).ProjectId == 0)
                {

                }
                else
                {
                    pm.Project = p.GetProject(username, projectid);
                  
                }
                pm.ProjectNotes.Roles = p.getRoles(projectid);
                pm.GetUsers = p.getUsers(projectid);
                pm.User = p.getUser(username);
                return View(pm);
            }
        }

     

        [HttpPost]
        public ActionResult DeleteTask(IEnumerable<int> TaskSelectedArray, int? id)
        {
         
            // Get values from session 
            string username = Session["username"].ToString();

             var result = "Tasks have been deleted";

            // Will error if task selected is nil
            if (TaskSelectedArray == null || !TaskSelectedArray.Any())
            {

                
                    result = "You have not selected any tasks to delete";
                    Debug.WriteLine("Nill");
                    return RedirectToAction("Index", "Project", result);


            }
            else
            {
                ProjectPageModel projectTask = new ProjectPageModel();
                for (int i = 0; i < TaskSelectedArray.Count(); i++)
                {
                    //projectTask.deleteTask(Convert.ToInt32(TaskSelectedArray[i]));
                    projectTask.deleteTask(TaskSelectedArray.ElementAt(i));
                }
            }


            return RedirectToAction("Index", "Project", result);
        }



        public JsonResult TaskDetail(string myTaskID)
        {

                ProjectPageModel taskModelSelect = new ProjectPageModel();
         
               
                Task pn = new Task();
                pn = taskModelSelect.getTaskDetail(Convert.ToInt32(myTaskID));
                return Json(pn, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ProjectNotesDetail(string myNoteID)
        {
            ProjectPageModel taskModelSelect = new ProjectPageModel();

        
            ProjectNotes pn = new ProjectNotes();
            pn = taskModelSelect.getProjectNotesDetails(Convert.ToInt32(myNoteID));
            return Json(pn, JsonRequestBehavior.AllowGet);
        }

        // Gets executed on Modal AddTask POST 
        [HttpPost]
        public ActionResult AddTask()
        {
            try
            {
                string username = "";
                // Retriving values from POST 
                string taskTitle = Request["taskTitle"];
                string taskDescription = Request["taskDescription"];
                string taskStartDate = Request["taskStartDate"];
                string taskEndDate = Request["taskEndDate"];
                //string taskCompletionDate = Request["taskCompletionDate"];
                string taskPriority = Request["taskPriority"];
                int id = Convert.ToInt32(Request["projectId"]);

                username= Request["assignUser"];




                Debug.WriteLine("username before it is empty or noh" + username);
                if (string.IsNullOrEmpty(username) || username == "0")
                {
                    Debug.WriteLine("run" + username);
                    username = Session["username"].ToString();
                }

                // Check Results from Posted values 
               // Debug.WriteLine("Task title " + taskTitle);
                //Debug.WriteLine("Task Description " + taskDescription);
                //Debug.WriteLine("Task StartDate " + taskStartDate);
                //Debug.WriteLine("Task EndDate " + taskEndDate);
                ////Debug.WriteLine("Task CompletionDate " + taskCompletionDate);
                //Debug.WriteLine("Task Priority " + taskPriority);
                //Debug.WriteLine("Task ProjectId " + id);

                DateTime dateTimeStartDate = DateTime.Parse(taskStartDate);
                string mdateTimeStartDate = dateTimeStartDate.ToString("dd/MM/yyyy");
                DateTime mtaskStartDate = DateTime.ParseExact(mdateTimeStartDate, "dd/MM/yyyy", null);

                DateTime dateTimeEndDate = DateTime.Parse(taskEndDate);
                string mdateTimeEndDate = dateTimeEndDate.ToString("dd/MM/yyyy");
                DateTime mtaskEndDate = DateTime.ParseExact(mdateTimeEndDate, "dd/MM/yyyy", null);

                //DateTime dateTimeCompletionDate = DateTime.Parse(taskCompletionDate);
                //string mdateTimeCompletionDate = dateTimeCompletionDate.ToString("dd-MM-yyyy");
                //DateTime mtaskCompletionDate = DateTime.ParseExact(mdateTimeCompletionDate, "dd/MM/yyyy", null);

              

                ProjectPageModel taskModel = new ProjectPageModel();
                Task task = new Task
                {
                    Title = taskTitle,
                    Description = taskDescription,
                    StartDate = mtaskStartDate,
                    DueDate = mtaskEndDate,
                    //CompletedDate = mtaskCompletionDate,
                    Priority = taskPriority,
                    ProjectId = id.ToString()
                };

                bool result = taskModel.AddTask(task, username);


                //Debug.WriteLine("THIS IS PROJECT id" + id);
                return RedirectToAction("Index", "Project", new { result, @id = id });
            }
            catch (Exception e)
            {
                var result = "false";    
                return RedirectToAction("Index", "Project", new { result});
            }
           
        }

        [HttpPost]
        public ActionResult EditTask(string[] editTaskArray)
        {

            try
            {
                // Retriving values from POST 
                /*
                string taskId = Request["editTaskId"];
                string taskTitle = Request["editTaskTitle"];
                string taskDescription = Request["editTaskDescription"];
                string taskStartDate = Request["editTaskStartDate"];
                string taskEndDate = Request["editTaskEndDate"];
                string taskCompletionDate = Request["editTaskCompletionDate"];
                string taskPriority = Request["editTaskPriority"];
                */

                string taskId = editTaskArray[0];
                string taskTitle = editTaskArray[1];
                string taskDescription = editTaskArray[2];
                string taskStartDate = editTaskArray[3];
                string taskEndDate = editTaskArray[4];
                string taskCompletionDate = editTaskArray[5];
                string taskPriority = editTaskArray[6];

                Debug.WriteLine("TASK ID TASK ID" + taskId);

                DateTime dateTimeStartDate = DateTime.Parse(taskStartDate);
                string mdateTimeStartDate = dateTimeStartDate.ToString("dd/MM/yyyy");
                DateTime mtaskStartDate = DateTime.ParseExact(mdateTimeStartDate, "dd/MM/yyyy", null);

                DateTime dateTimeEndDate = DateTime.Parse(taskEndDate);
                string mdateTimeEndDate = dateTimeEndDate.ToString("dd/MM/yyyy");
                DateTime mtaskEndDate = DateTime.ParseExact(mdateTimeEndDate, "dd/MM/yyyy", null);

                DateTime dateTimeCompletionDate = DateTime.Parse(taskCompletionDate);
                string mdateTimeCompletionDate = dateTimeCompletionDate.ToString("dd/MM/yyyy");
                DateTime mtaskCompletionDate = DateTime.ParseExact(mdateTimeCompletionDate, "dd/MM/yyyy", null);

                string username = Session["username"].ToString();
                // Get values from session 

                ProjectPageModel taskModel = new ProjectPageModel();
                Task task = new Task
                {
                    Title = taskTitle,
                    Description = taskDescription,
                    StartDate = mtaskStartDate,
                    DueDate = mtaskEndDate,
                    CompletedDate = mtaskCompletionDate,
                    Priority = taskPriority,
                    TaskId = Convert.ToInt32(taskId)
                };

                bool result = taskModel.updateTask(Convert.ToInt32(taskId), task);
                return RedirectToAction("Index", "Project", result);
            }
            catch (Exception e)
            {
                bool result = false; 
                return RedirectToAction("Index", "Project", result);
            }

        }

        [HttpPost]
        public ActionResult DeleteProjectNote(IEnumerable<int> ProjectNoteSelectedArray, int? id)
        {
            // Get values from session 
            string username = Session["username"].ToString();

            var result = "Tasks have been deleted";

            // Will error if project note selected is nil
            if (ProjectNoteSelectedArray == null || !ProjectNoteSelectedArray.Any())
            {
                result = "You have not selected any tasks to delete";
                return RedirectToAction("Index", "Project", result);
            }
            else
            {
                ProjectPageModel projectNote = new ProjectPageModel();
                for (int i = 0; i < ProjectNoteSelectedArray.Count(); i++)
                {
                    projectNote.DeleteProjectNotes(ProjectNoteSelectedArray.ElementAt(i));
                }
            }


            return RedirectToAction("Index", "Project", result);
        }

        [HttpPost]
        public ActionResult EditProjectNotes(string[] editProjectNotesArray)
        {

            try
            {
               

                string projectNotesId = editProjectNotesArray[0];
                string projectNotesMessage = editProjectNotesArray[1];
                //string projectNotesCompletionDate = editProjectNotesArray[2];
                string projectNotesTo = editProjectNotesArray[3];
                
               // DateTime dateTimeCompletionDate = DateTime.Parse(projectNotesCompletionDate);
                //string mdateTimeCompletionDate = dateTimeCompletionDate.ToString("dd/MM/yyyy");
                //DateTime mtaskCompletionDate = DateTime.ParseExact(mdateTimeCompletionDate, "dd/MM/yyyy", null);

                string username = Session["username"].ToString();
                // Get values from session 

                Debug.WriteLine(projectNotesId);
                Debug.WriteLine(projectNotesMessage);
               // Debug.WriteLine(projectNotesCompletionDate);
                Debug.WriteLine(projectNotesTo);

                ProjectPageModel projectNotesModel = new ProjectPageModel();
                ProjectNotes projectNotes = new ProjectNotes
                {
                    
                    Message = projectNotesMessage,
                   // CompletedDate = mtaskCompletionDate,
                    To = projectNotesTo,
                    ProjectNoteId = Convert.ToInt32(projectNotesId)
                };

                bool result = projectNotesModel.updateProjectNotes(Convert.ToInt32(projectNotesId), projectNotes);
                return RedirectToAction("Index", "Project", result);
            }
            catch (Exception e)
            {
                bool result = false;
                return RedirectToAction("Index", "Project", result);
            }

        }


        [HttpPost]
        public ActionResult AddProjectNote(int? projectId)
        {
            // Get values from session 

            string username = Session["username"].ToString();
            int id = Convert.ToInt32(projectId);
            bool result = false; 

            // Validations needed 
            try
            {
                string projectNoteMessage = Request["noteMessage"];
                string projectNoteSentDate = DateTime.Now.ToString();
                //string projectNoteCompletedDate = Request["noteCompletedDate"];
                string projectNoteTo = Request["noteTo"];

                DateTime dateTimeSentDate = DateTime.Parse(projectNoteSentDate);
                string mdateTimeSentDate = dateTimeSentDate.ToString("dd/MM/yyyy");
                DateTime mprojectSentDate = DateTime.ParseExact(mdateTimeSentDate, "dd/MM/yyyy", null);


             //   DateTime dateTimeCompletedDate = DateTime.Parse(projectNoteCompletedDate);
             //   string mdateTimeCompletedDate = dateTimeCompletedDate.ToString("dd/MM/yyyy");
              //  DateTime mprojectCompletedDate = DateTime.ParseExact(mdateTimeCompletedDate, "dd/MM/yyyy", null);

         

                ProjectPageModel pm = new ProjectPageModel();
                ProjectNotes projectNotes = new ProjectNotes
                {
                    ProjectId = Convert.ToInt32(id),
                    Message = projectNoteMessage,
                    SentDate = mprojectSentDate,
                    From = username,
                    To = projectNoteTo,
                   
                };
                Debug.WriteLine("Project ID" + projectNotes.ProjectId);
                Debug.WriteLine("Message" + projectNotes.Message);
                Debug.WriteLine("SentDate " + projectNotes.SentDate);

                Debug.WriteLine("From " + projectNotes.From);
                Debug.WriteLine("To " + projectNotes.To);
               result = pm.AddProjectNote(projectNotes);
            }
            catch(Exception e)
            {

            }
            
            
            return RedirectToAction("Index", "Project", new { result, @id = id });
        }

        [HttpPost]
        public JsonResult getRoles(int projectId)
        {
            ProjectPageModel projectNotesModel = new ProjectPageModel();

            string[] strList = projectNotesModel.getRoles(projectId);

            return Json(new { strList }, JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public ActionResult isCompleted(int? projectId, int taskID, bool complete)
        {
            int id = Convert.ToInt32(projectId);

            // Gets values from POST request 
            //int taskID = Convert.ToInt32(Request["taskID"]);
            //bool complete = Convert.ToBoolean(Request["isCompleted"]);
           // Debug.WriteLine("Task ID = " + taskID, " isCompleted  = " + complete);
            // Call SQL method
            ProjectPageModel projectPage = new ProjectPageModel();
            var result = projectPage.isCompleted(complete, taskID);

            return RedirectToAction("Index", "Project", new { result, @id = id });
        }

        public JsonResult TaskEditView(IEnumerable<int> TaskSelectedArray)
        {
            ProjectPageModel taskModelSelect = new ProjectPageModel();
            if (TaskSelectedArray == null || !TaskSelectedArray.Any())
            {

                // WILL ERROR IF YOU RUN THIS CONTROLLER 

                //return JsonConvert.SerializeObject(td);
                //return td;
                return Json(JsonRequestBehavior.AllowGet);

            }
            else
            {

                Task td = new Task();
                td = taskModelSelect.getSelectEditTask(TaskSelectedArray.ElementAt(0));
                return Json(td, JsonRequestBehavior.AllowGet);

            }
            
        }


        public ActionResult TaskView(int? projectId)
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            ProjectPageModel taskModel = new ProjectPageModel();
            string username = Session["username"].ToString();
         
            int id = Convert.ToInt32(projectId);
            IEnumerable<ProjectPageViewModel> td = taskModel.getTask(username, id);
            return View(td);
        }

        public ActionResult TaskViewPM(int? projectId)
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            ProjectPageModel taskModel = new ProjectPageModel();
            string username = Session["username"].ToString();

            int id = Convert.ToInt32(projectId);
            IEnumerable<ProjectPageViewModel> td = taskModel.getTaskPM(id);
            return View(td);
        }


        public ActionResult NoteView(int? projectId, string roleName)
        {

            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            string username = Session["username"].ToString();
            int id = Convert.ToInt32(projectId);

            ProjectPageModel projectNotesModel = new ProjectPageModel();
            IEnumerable<ProjectPageViewModel> td = projectNotesModel.getProjectNotes(username, id, roleName);
            return View(td);
        }

        public ActionResult NoteViewPM(int? projectId, string roleName)
        {

            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            string username = Session["username"].ToString();
            int id = Convert.ToInt32(projectId);

            ProjectPageModel projectNotesModel = new ProjectPageModel();
            IEnumerable<ProjectPageViewModel> td = projectNotesModel.getProjectNotesPM(username, id, roleName);
            return View(td);
        }

        public JsonResult ProjectNotesEditView(IEnumerable<int> ProjectNoteSelectedArray)
        {
            ProjectPageModel taskModelSelect = new ProjectPageModel();
            if (ProjectNoteSelectedArray == null || !ProjectNoteSelectedArray.Any())
            {

                // WILL ERROR IF YOU RUN THIS CONTROLLER 

                
                //return td;
                return Json(JsonRequestBehavior.AllowGet);

            }
            else
            {
               
                ProjectNotes pn = new ProjectNotes();
                pn = taskModelSelect.getSelectEditProjectNotes(ProjectNoteSelectedArray.ElementAt(0));
                return Json(pn, JsonRequestBehavior.AllowGet);

            }

        }


        public ActionResult DocumentView(int? projectId, string roleName, string uName)
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            // Pass roleName from Main Model view to document view 
            string username = Session["username"].ToString();
            int id = Convert.ToInt32(projectId);
           // Debug.WriteLine("ROLE NAME IS: " + roleName);
            ProjectPageModel documentModel = new ProjectPageModel();
           
            IEnumerable<ProjectPageViewModel> td = documentModel.getDocuments(username, id, roleName, uName);
            return View(td);
        }


        public ActionResult DocumentViewPM(int? projectId, string roleName, string uName)
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Dashboard");
            }

            // Pass roleName from Main Model view to document view 
            string username = Session["username"].ToString();
            int id = Convert.ToInt32(projectId);
            // Debug.WriteLine("ROLE NAME IS: " + roleName);
            ProjectPageModel documentModel = new ProjectPageModel();

            IEnumerable<ProjectPageViewModel> td = documentModel.getDocumentsPM(username, id, roleName, uName);
            return View(td);
        }




    }
}