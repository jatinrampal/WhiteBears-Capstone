using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class ProjectPageModel
    {
        private string connString = @"Data Source=whitebears-server.database.windows.net;Initial Catalog=whitebears-db;Persist Security Info=True;User ID=sysdba;Password=j3.'(Ge=";

        public IEnumerable<ProjectPageViewModel> getDocuments(String username, int projectId, string userRoleName)
        {
            //var model = new Task();
            //List<ProjectDocumentViewModel> modelList = new List<ProjectDocumentViewModel>();
            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                //command.CommandText = "select Document.documentId, Document.projectId, Document.fileName, Document.uploader, Document.creationTime, Document.fileExtension, DocumentRole.writeAccess, DocumentRole.roleName from  Document LEFT JOIN DocumentRole ON Document.documentId = DocumentRole.documentId WHERE projectId = @projectId; ";

                command.CommandText = "SELECT d.documentId, d.projectId, d.uploader, d.creationTime, d.fileExtension, d.fileName, dr.writeAccess, dr.roleName, u.firstName FROM DocumentRole dr JOIN Document d ON @projectId = dr.documentId JOIN[User] u ON u.[role] = dr.roleName WHERE dr.roleName = @rolename";

                /*
                SELECT d.documentId, d.fileExtension, d.fileName, dr.roleName, u.firstName FROM DocumentRole dr
                JOIN Document d ON d.documentId = dr.documentId
                JOIN[User] u ON u.[role] = dr.roleName
                WHERE dr.roleName = 'Full Stack Developer'
                */

                //command.CommandText = "select * from Document where projectId = @projectId";
                command.Parameters.AddWithValue("@rolename", userRoleName);
                command.Parameters.AddWithValue("@projectId", projectId);

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

        public IEnumerable<ProjectPageViewModel> getTask(String username, int projectId)
        {
            //var model = new Task();
            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "SELECT * FROM Task t JOIN Project p on t.projectId = p.projectId JOIN User_Task ut on t.taskId = ut.taskId WHERE ut.uName = @username and p.projectId = @projectId";
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
                    modelList.Add(model);
                }

                reader.Close();
                connection.Close(); 
            }
            return modelList;
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
                dh.RunUpdateQuery($"INSERT INTO Task (title, description, startDate, dueDate, completionDate, projectId, priority) VALUES('{task.Title}', '{task.Description}', '{task.StartDate}'," +
                    $"'{task.DueDate}', '{task.CompletedDate}', '{task.ProjectId}', '{task.Priority}');");

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

        public IEnumerable<ProjectPageViewModel> getProjectNotes(String username, int projectId)
        {

            List<ProjectPageViewModel> modelList = new List<ProjectPageViewModel>();

            using (SqlConnection connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand("", connection))
            {

                // Query with vals 
                command.CommandText = "SELECT * FROM ProjectNote WHERE projectId = @projectId";
                command.Parameters.AddWithValue("@projectId", projectId);

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
    }

}
