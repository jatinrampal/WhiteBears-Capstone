using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WhiteBears.Models
{
    public class ProjectPageModel
    {
        private string connString = @"Data Source=whitebears-server.database.windows.net;Initial Catalog=whitebears-db;Persist Security Info=True;User ID=sysdba;Password=j3.'(Ge=";

        public IEnumerable<ProjectPageViewModel> getDocuments(String username, int projectId, string userRoleName, string uName)
        {
            //var model = new Task();
            //List<ProjectDocumentViewModel> modelList = new List<ProjectDocumentViewModel>();
            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                //command.CommandText = "select Document.documentId, Document.projectId, Document.fileName, Document.uploader, Document.creationTime, Document.fileExtension, DocumentRole.writeAccess, DocumentRole.roleName from  Document LEFT JOIN DocumentRole ON Document.documentId = DocumentRole.documentId WHERE projectId = @projectId; ";

                command.CommandText = "select DISTINCT(d.filename), d.projectId, d.fileExtension, d.uploader, d.creationTime, dr.writeAccess, d.documentId, dr.roleName from Document d JOIN DocumentRole dr ON  d.documentId = dr.documentId JOIN[User] ON[User].role = dr.roleName where projectId = @projectId AND dr.roleName = @userRoleName AND [User].uName = @uName AND dr.writeAccess = 1;";

             
                //command.CommandText = "select * from Document where projectId = @projectId";
                command.Parameters.AddWithValue("@userRolename", userRoleName);
                command.Parameters.AddWithValue("@projectId", projectId);
                command.Parameters.AddWithValue("@uName", uName);

                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Multiple rows 
                while (reader.Read())
                {
                    //var model = new ProjectDocumentViewModel();
                    var model = new ProjectPageViewModel();
                    var docID = Convert.ToInt32(reader["documentId"]);

                    model.document.ProjectId = Convert.ToInt32(reader["projectId"]);
                    model.document.DocumentId = Convert.ToInt32(reader["documentId"]);
                    model.document.FileName = reader["fileName"].ToString();
                    model.document.Uploader = reader["uploader"].ToString();
                    model.document.CreationTime = (DateTime)reader["creationTime"];
                    model.document.FileExtension = reader["fileExtension"].ToString();

                    if (reader["roleName"] != DBNull.Value)
                    {
                        model.documentRole.RoleName = reader["roleName"].ToString();
                    }
                    else
                    {
                        model.documentRole.RoleName = "No Role";
                    }

                    if (reader["writeAccess"] != DBNull.Value)
                    {
                        model.documentRole.WriteAccess = Boolean.Parse(reader["writeAccess"].ToString());
                    }
                    else
                    {
                        model.documentRole.WriteAccess = false;
                    }

                    if (reader["roleName"].ToString() == userRoleName)
                    {
                        model.documentRole.WriteAccess = true;
                    }
                    modelList.Add(model);
                }
                reader.Close();
                connection.Close();
            }
            return modelList;
        }

        public Task getSelectEditTask(int taskId)
        {
          Task modelList = new Task();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "SELECT * FROM Task WHERE taskId = @taskId;";
                command.Parameters.AddWithValue("@taskId", taskId);


                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Multiple rows 
                while (reader.Read())
                {
                    var model = new ProjectPageViewModel();

                    modelList.TaskId = model.Task.TaskId = Convert.ToInt32(reader["TaskId"]);
                    modelList.Title = model.Task.Title = reader["Title"].ToString();
                    modelList.CompletedDate = model.Task.CompletedDate = (DateTime)reader["completionDate"];
                    modelList.StartDate = model.Task.StartDate = (DateTime)reader["startDate"];
                    modelList.DueDate =  model.Task.DueDate = (DateTime)reader["dueDate"];
                    modelList.Priority = model.Task.Priority = reader["priority"].ToString();
                    modelList.Description = model.Task.Description = reader["description"].ToString();
                    
                }

                reader.Close();
                connection.Close();
            }
            return modelList;
        }

        public ProjectNotes getSelectEditProjectNotes(int projectNoteId)
        {
            ProjectNotes modelList = new ProjectNotes();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "SELECT * FROM ProjectNote WHERE projectNoteId = @projectNoteId;";
                command.Parameters.AddWithValue("@projectNoteId", projectNoteId);


                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Multiple rows 
                while (reader.Read())
                {
                    var model = new ProjectPageViewModel();

                    modelList.ProjectNoteId = Convert.ToInt32(reader["projectNoteId"]);
                    modelList.ProjectId = Convert.ToInt32(reader["projectId"]);
                    modelList.Message = reader["message"].ToString();
                    modelList.SentDate = Convert.ToDateTime(reader["sentDate"]);
                    modelList.From = reader["from"].ToString();
                    modelList.To = reader["to"].ToString();

                    if (reader["completedDate"] != DBNull.Value)
                    {
                        modelList.CompletedDate = Convert.ToDateTime(reader["completedDate"]);
                    }
                    else
                    {
                        modelList.CompletedDate = DateTime.MinValue;
                    }

                }

                reader.Close();
                connection.Close();
            }
            return modelList;
        }


        public bool updateTask(int taskId, Task task)
        {
           
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                var title = task.Title;
                var description = task.Description;
                var startDate = task.StartDate;
                var dueDate = task.DueDate;
                var completionDate = task.CompletedDate;
                var priority = task.Priority; 

                ///dh.RunUpdateQuery($"DELETE FROM ProjectNote WHERE projectNoteId='{projectNoteId}'");
                dh.RunUpdateQuery($"UPDATE Task set Title='{title}', [description]='{description}', startDate='{startDate}', dueDate='{dueDate}', completionDate='{completionDate}', priority='{priority}' WHERE taskId = {taskId}");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool updateProjectNotes(int projectNotesId, ProjectNotes projectNotes)
        {

            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                var message = projectNotes.Message;
                var completionDate = projectNotes.CompletedDate;
                var to = projectNotes.To;

                ///dh.RunUpdateQuery($"DELETE FROM ProjectNote WHERE projectNoteId='{projectNoteId}'");
                dh.RunUpdateQuery($"UPDATE ProjectNote set message='{message}', completedDate='{completionDate}', [to]='{to}' WHERE projectNoteId = '{projectNotesId}'");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public IEnumerable<ProjectPageViewModel> getTask(String username, int projectId)
        {
            //var model = new Task();
            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "SELECT * FROM Task t JOIN Project p on t.projectId = p.projectId JOIN User_Task ut on t.taskId = ut.taskId WHERE ut.uName = @username and p.projectId = @projectId ORDER BY cast(T.dueDate as datetime) asc";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@projectId", projectId);

                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Multiple rows 
                while (reader.Read())
                {
                    var model = new ProjectPageViewModel();

                    model.Task.TaskId = Convert.ToInt32(reader["TaskId"]);
                    model.Task.Title = reader["Title"].ToString();
                    model.Task.CompletedDate = (DateTime)reader["completionDate"];

                    model.Task.DueDate = (DateTime)reader["dueDate"];
                    model.Task.Priority = reader["priority"].ToString();
                    model.Task.Description = reader["description"].ToString();
                    modelList.Add(model);
                }

                reader.Close();
                connection.Close(); 
            }
            return modelList;
        }
       public bool isCompleted(bool complete, int taskID)
        {

            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                
                DateTime currentDateTime = DateTime.Now;
                if (complete == true)
                {
                    Debug.WriteLine("Complee value being passed is " + complete + " With Date Time " + currentDateTime);
                    dh.RunUpdateQuery($"UPDATE Task SET Task.completionDate = '{currentDateTime}' where taskId = '{taskID}'");
                }
                else if (complete == false)
                {
                    Debug.WriteLine("Complee value being passed is " + complete + " but failed");
                    dh.RunUpdateQuery($"UPDATE Task SET Task.completionDate = '0001-01-01' where taskId = '{taskID}'");
                }
            
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

           
        }

        public bool deleteTask(int taskId)
        {

            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunUpdateQuery($"DELETE FROM User_Task WHERE taskId='{taskId}'");
                dh.RunUpdateQuery($"DELETE FROM Task WHERE taskId='{taskId}'");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddTask(Task task, string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                // Keeps the database not NULL (As it will effect Dashboard tasks)
                DateTime completionDate = DateTime.ParseExact("0001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                dh.RunUpdateQuery($"INSERT INTO Task (title, description, startDate, dueDate, completionDate, projectId, priority) VALUES('{task.Title}', '{task.Description}', '{task.StartDate}'," +
                    $"'{task.DueDate}', '{completionDate}', '{task.ProjectId}', '{task.Priority}');");

                DataRow[] drs = dh.RunQuery($"SELECT MAX(taskId) FROM Task;");
                dh.RunUpdateQuery($"INSERT INTO User_Task VALUES('{username}', '{drs[0][0].ToString()}');");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public IEnumerable<ProjectPageViewModel> getProjectNotes(String username, int projectId, string roleName)
        {

            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                //command.CommandText = "SELECT * FROM ProjectNote WHERE projectId = @projectId ";
                command.CommandText = "SELECT * FROM ProjectNote pn WHERE projectId = @projectId AND [to] = @userRole ORDER BY sentDate; ";
                command.Parameters.AddWithValue("@projectId", projectId);
                command.Parameters.AddWithValue("@userRole", roleName);

                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Multiple rows 
                while (reader.Read())
                {
                    var model = new ProjectPageViewModel();

                    model.ProjectNotes.ProjectNoteId = Convert.ToInt32(reader["projectNoteId"]);
                    model.ProjectNotes.ProjectId = Convert.ToInt32(reader["projectId"]);
                    model.ProjectNotes.Message = reader["message"].ToString();
                    model.ProjectNotes.SentDate = Convert.ToDateTime(reader["sentDate"]);
                    model.ProjectNotes.From = reader["from"].ToString();
                    model.ProjectNotes.To = reader["to"].ToString();

                    if (reader["completedDate"] != DBNull.Value)
                    {
                        model.ProjectNotes.CompletedDate = Convert.ToDateTime(reader["completedDate"]);
                    }
                    else
                    {
                        model.ProjectNotes.CompletedDate = DateTime.MinValue;
                    }
                    modelList.Add(model);
                }


                reader.Close();
                connection.Close();
            }
            return modelList;
        }

        public List<string> getRoles()
        {
            var listOfRoles = new List<string>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "select role from [User]; ";

                // Open connection 
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string roleVal = reader["role"].ToString();
                    listOfRoles.Add(roleVal);

                }
            }

            return listOfRoles;
        }

        public bool AddProjectNote(ProjectNotes note)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunUpdateQuery($"INSERT INTO ProjectNote (projectId, message, sentDate, [from], [to], completedDate) VALUES('{note.ProjectId}', '{note.Message}', " +
                    $"'{note.SentDate}', '{note.From}', '{note.To}', '{note.CompletedDate}');");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteProjectNotes(int projectNoteId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                dh.RunUpdateQuery($"DELETE FROM ProjectNote WHERE projectNoteId='{projectNoteId}'");

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public Project GetProject(string currentUser, int projectId)
        {
            try
            {
                DatabaseHelper dh = new DatabaseHelper();
                DataRow[] drs = dh.RunQuery($"SELECT * FROM Project WHERE projectId = '{projectId}';");

                return new Project()
                {
                    ProjectId = Int32.Parse(drs[0]["projectId"].ToString()),
                    Title = drs[0]["title"].ToString(),
                    Description = drs[0]["description"].ToString(),
                    StartDate = Convert.ToDateTime(drs[0]["startDate"]),
                    DueDate = Convert.ToDateTime(drs[0]["dueDate"]),
                    CompletionDate = Convert.ToDateTime(drs[0]["completionDate"])
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine("Crash: " + e);
                return new Project()
                {
                    ProjectId = 0,
                    Title = "0",
                    Description = "0",
                    StartDate = Convert.ToDateTime("0001-01-01"),
                    DueDate = Convert.ToDateTime("0001-01-01"),
                    CompletionDate = Convert.ToDateTime("0001-01-01")

                };
            }
           
        }

        public User getUser(string currentUser)
        {
            DatabaseHelper dh = new DatabaseHelper();
            DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{currentUser}';");

            return new User()
            {
                Username = drs[0]["uName"].ToString(),
                FirstName = drs[0]["firstName"].ToString(),
                LastName = drs[0]["lastName"].ToString(),
                Role = drs[0]["role"].ToString(),
                FullName = drs[0]["firstName"].ToString() + " "+ drs[0]["lastName"].ToString(),
            };
        }

        public bool userAccessProject(int projectId, string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM Project p JOIN User_Project up ON up.projectId = p.projectId where p.projectId = '{projectId}' AND up.uName = '{username}';");
                if(drs.Length == 0)
                {
                    return false; 
                }
            }
            catch(Exception e)
            {
                return false; 
            }
            return true; 
            //SELECT * FROM Project p JOIN User_Project up ON up.projectId = p.projectId where p.projectId = 2 AND up.uName = 'Kish';  
        }

        public Task currTaskSelect(int projectId, int taskId)
        {
            Task task = new Task();
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM Task t where t.taskId = '{taskId}' AND t.projectId = '{projectId}';");
                return new Task()
                {
                    TaskId = Convert.ToInt32(drs[0]["taskId"]),
                    Title = drs[0]["title"].ToString(),
                    Description = drs[0]["description"].ToString(),
                    StartDate = Convert.ToDateTime(drs[0]["startDate"].ToString()),
                    DueDate = Convert.ToDateTime(drs[0]["dueDate"].ToString()),
                    CompletedDate = Convert.ToDateTime(drs[0]["completionDate"].ToString()),
                    ProjectId = drs[0]["projectId"].ToString(),
                    Priority = drs[0]["priority"].ToString()


                };
            }
            catch(Exception e)
            {

                return task; 
            }
        }
    }

}
