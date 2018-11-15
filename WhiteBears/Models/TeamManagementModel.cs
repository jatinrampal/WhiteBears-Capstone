using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class TeamManagementModel
    {
        public User CurrentUser { get; set; }
        public User[] ExcludedUsers { get; set; }
        public User[] IncludedUsers { get; set; }
        public Project CurrentProject { get; set; }
        public Project[] Projects { get; set; }
        public DateTime currDate = DateTime.Now;

        public DateTime CurrDate { get { return currDate; } }


        public Project[] GetProjects(string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            DataRow[] drs = dh.RunQuery($"SELECT * FROM Project p " +
                    $"INNER JOIN User_Project up ON p.projectId = up.projectId " +
                    $"WHERE up.uName = '{username}';");

            int i = 0;

            Project[] projects = new Project[drs.Count()];

            foreach(DataRow dr in drs){
                projects[i++] = new Project()
                {
                    ProjectId = Int32.Parse(dr["projectId"].ToString()),
                    Title = dr["title"].ToString(),
                    Description = dr["description"].ToString(),
                    ScopeStatement = dr["scopeStatement"].ToString(),
                    StartDate = Convert.ToDateTime(dr["startDate"]),
                    DueDate = Convert.ToDateTime(dr["dueDate"].ToString()),
                    CompletionDate = Convert.ToDateTime(dr["completionDate"].ToString())
                };
            }

            return projects;
        }

        public User[] GetIncludedUsers(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT DISTINCT u.uName, * FROM [User] u " +
                    $"JOIN User_Project up ON u.uName = up.uName " +
                    $"WHERE up.projectId='{projectId}';");

                List<User> users = new List<User>();

                foreach (DataRow dr in drs)
                {
                    if(Convert.ToBoolean(dr["enabled".ToString()]))
                        users.Add(new User(dr["firstName"].ToString(), dr["lastName"].ToString(), dr["uName"].ToString(), $"{dr["firstName"].ToString()}@email.com", dr["password"].ToString(), dr["role"].ToString()));
                }

                return users.ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public User[] GetExcludedUsers(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] " +
                    $"WHERE uName NOT IN " +
                    $"(SELECT u.uName FROM [User] u " +
                    $"JOIN User_Project up ON u.uName = up.uName " +
                    $"WHERE up.projectId='{projectId}');");

                List<User> users = new List<User>();

                foreach (DataRow dr in drs)
                {
                    //TODO: refactor email to dr["email"]
                    if (Convert.ToBoolean(dr["enabled".ToString()]))
                        users.Add(new User(dr["firstName"].ToString(), dr["lastName"].ToString(), dr["uName"].ToString(), $"{dr["firstName"].ToString()}@email", dr["password"].ToString(), dr["role"].ToString()));
                }

                return users.ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public Project GetProject(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [Project] WHERE projectId='{projectId}';");

                return new Project()
                {
                    ProjectId = Int32.Parse(drs[0]["projectId"].ToString()),
                    Title = drs[0]["title"].ToString(),
                    Description = drs[0]["description"].ToString(),
                    ScopeStatement = drs[0]["scopeStatement"].ToString(),
                    StartDate = Convert.ToDateTime(drs[0]["startDate"]),
                    DueDate = Convert.ToDateTime(drs[0]["dueDate"].ToString()),
                    CompletionDate = Convert.ToDateTime(drs[0]["completionDate"].ToString())
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public User GetUser(string username){
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] WHERE uName='{username}';");
                //TODO: Refactor email to drs[0][email].toString()
                return new User(drs[0]["firstName"].ToString(), drs[0]["lastName"].ToString(), drs[0]["uName"].ToString(), $"{drs[0]["firstName"].ToString()}@email.com", drs[0]["password"].ToString(), drs[0]["role"].ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static bool AddUserToProject(string[] usernames, int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();

            try
            {
                foreach (string username in usernames)
                {


                    if (dh.RunQuery($"SELECT uName FROM [User] WHERE uName='{username}'").Count() != 1)
                        continue;

                    if (dh.RunQuery($"SELECT projectId FROM [Project] WHERE projectId='{projectId}'").Count() != 1)
                        continue;

                    if (dh.RunQuery($"SELECT uName, ProjectId FROM [User_Project] " +
                        $"WHERE projectId='{projectId}' AND uName='{username}'").Count() == 1)
                        continue;

                    dh.RunUpdateQuery($"INSERT INTO User_Project VALUES('{username}', '{projectId}');");
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool RemoveUserFromProject(string[] usernames, int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();

            try
            {
                foreach (string username in usernames)
                {
                    if (dh.RunQuery($"SELECT uName, ProjectId FROM [User_Project] " +
                        $"WHERE projectId='{projectId}' AND uName='{username}'").Count() < 1)
                        continue;

                    dh.RunUpdateQuery($"DELETE FROM User_Project " +
                        $"WHERE projectId='{projectId}' AND uName='{username}'");
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}