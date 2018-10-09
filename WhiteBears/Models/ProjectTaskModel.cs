using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class ProjectTaskModel
    {
        private string connString = @"Data Source=whitebears-server.database.windows.net;Initial Catalog=whitebears-db;Persist Security Info=True;User ID=sysdba;Password=j3.'(Ge=";

        public IEnumerable<Task> getTask(String username, int projectId)
        {
            //var model = new Task();
            List<Task> modelList = new List<Task>();

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
                    var model = new Task();
                    model.TaskId = Convert.ToInt32(reader["TaskId"]);
                    model.Title = reader["Title"].ToString();
                    model.CompletedDate = (DateTime)reader["completionDate"];
                   
                    model.DueDate = (DateTime)reader["dueDate"];
                    modelList.Add(model);
                }

                
                reader.Close();
                connection.Close();
                /*
                command.CommandText = "select * from Names where Id=@Id";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                model.Id = id;
                model.Name = reader["Name"].ToString();
                */
            }
            return modelList;
        }
    }
}