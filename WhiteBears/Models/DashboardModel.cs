using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using WhiteBears;

namespace WhiteBears.Models {
    public class DashboardModel {
        public Project[] Projects { get; set; }
        public User CurrentUser { get; set; }

        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrDate { get; set; }

        public DataRow[] GetUser(String username) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{username}';");
        }

        public string GetProjectName(string projectId) {
            DatabaseHelper dh = new DatabaseHelper();

            DataRow[] dr = dh.RunQuery($"SELECT title FROM Project WHERE projectId='{projectId}'");

            return dr[0]["title"].ToString();
        }

        public DataRow[] GetProjects(string username) {
            DatabaseHelper dh = new DatabaseHelper();

            return dh.RunQuery($"SELECT * FROM Project p " +
                $"INNER JOIN User_Project up ON p.projectId = up.projectId " +
                $"WHERE up.uName = '{username}';");
        }

        public Project GetProject(string username, string projectId) {
            DatabaseHelper dh = new DatabaseHelper();
            DataRow[] drs = dh.RunQuery($"SELECT * FROM Project p " +
                $"INNER JOIN User_Project up ON p.projectId = up.projectId " +
                $"WHERE up.uName = '{username}' " +
                                        $"AND p.projectId = '{projectId}';");


            DataRow[] drs1 = GetTasks(username, Int32.Parse(drs[0]["projectId"].ToString()));
            Task[] tasks = new Task[drs1.Count()];

            int i = 0;
            foreach (DataRow dr1 in drs1) {
                DateTime dueDate = Convert.ToDateTime(dr1["dueDate"]);
                DateTime completionDate = Convert.ToDateTime(dr1["completionDate"]);

                tasks[i++] = new Task {
                    Title = dr1["title"].ToString(),
                    Priority = dr1["priority"].ToString(),
                    DueDate = dueDate,
                    Status = DateTime.Now < dueDate ? "On time" : "Overdue",
                    ProjectName = drs[0]["title"].ToString(),
                    CompletedDate = completionDate
                };
            }

            return new Project {
                ProjectId = Int32.Parse(drs[0]["projectId"].ToString()),
                Title = drs[0]["title"].ToString(),
                Tasks = tasks
            };
        }

        public DataRow[] GetTasks(string sUser, int projectId) {
            DatabaseHelper dh = new DatabaseHelper();

            return dh.RunQuery($"SELECT * FROM Task t " +
                               $"JOIN Project p on t.projectId = p.projectId " +
                               $"JOIN User_Task ut on t.taskId = ut.taskId " +
                               $"WHERE ut.uName='{sUser}' " +
                               $"AND p.projectId='{projectId}'");
        }

        public DataRow[] GetAllTasks(int projectId) {
            DatabaseHelper dh = new DatabaseHelper();

            return dh.RunQuery($"SELECT * FROM Task t " +
                               $"JOIN Project p on t.projectId = p.projectId " +
                               $"AND p.projectId='{projectId}'");
        }

        public List<User> GetTaskUsers(int taskId) {
            DatabaseHelper dh = new DatabaseHelper();

            DataRow[] drs = dh.RunQuery($"SELECT u.FirstName, u.LastName FROM [User] u " +
                                "JOIN User_Task ut ON u.uName = ut.uName " +
                                $"WHERE ut.taskId = '{taskId}'");

            List<User> users = new List<User>();

            foreach(DataRow dr in drs) {
                users.Add(new User(dr["firstName"].ToString(), dr["lastName"].ToString()));
            }

            return users;
        }

        public DataRow[] GetPersonalNote(string uName) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM PersonalNote WHERE PersonalNote.uName = '{uName}';");
        }

        public DataRow[] AddPersonalNote(string uName, string note) {
            DatabaseHelper dh = new DatabaseHelper();
            dh.RunUpdateQuery($"INSERT INTO PersonalNote VALUES('{uName}', '{note}', '{DateTime.Now}');");

            return dh.RunQuery($"SELECT MAX(noteId) FROM PersonalNote");
        }

        public int DeletePersonalNote(int noteId) {
            DatabaseHelper dh = new DatabaseHelper();
            //DELETE FROM PersonalNote WHERE PersonalNote.uName = 'Kish' AND PersonalNote.noteId = 2;
            return dh.RunUpdateQuery($"DELETE FROM PersonalNote WHERE PersonalNote.noteId = '{noteId}';");
        }

        public int UpdatePersonalNote(string note, int noteId) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunUpdateQuery($"UPDATE PersonalNote SET note='{note}' WHERE PersonalNote.noteId = '{noteId}';");
        }
    }
}