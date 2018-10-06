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
        User[] ExcludedUsers;
        User[] IncludedUsers;
        Project currentProject;


        public static User[] GetIncludedUsers(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT DISTINCT u.uName, * FROM [User] u " +
                    $"JOIN User_Project up ON u.uName = up.uName " +
                    $"WHERE up.projectId='{projectId}';");

                User[] users = new User[drs.Count()];

                int i = 0;
                foreach (DataRow dr in drs)
                {
                    users[i++] = new User(dr["firstName"].ToString(), dr["lastName"].ToString(), dr["uName"].ToString(), dr["password"].ToString(), dr["role"].ToString());
                }

                return users;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static User[] GetExcludedUsers(int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            try
            {
                DataRow[] drs = dh.RunQuery($"SELECT * FROM [User] " +
                    $"WHERE uName NOT IN " +
                    $"(SELECT u.uName FROM [User] u " +
                    $"JOIN User_Project up ON u.uName = up.uName " +
                    $"WHERE up.projectId='{projectId}');");

                User[] users = new User[drs.Count()];

                int i = 0;
                foreach (DataRow dr in drs)
                {
                    users[i++] = new User(dr["firstName"].ToString(), dr["lastName"].ToString(), dr["uName"].ToString(), dr["password"].ToString(), dr["role"].ToString());
                }

                return users;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static bool AddUserToProject(string username, int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();

            try
            {
                if (dh.RunQuery($"SELECT uName FROM [User] WHERE uName='{username}'").Count() != 1)
                    return false;

                if (dh.RunQuery($"SELECT projectId FROM [Project] WHERE projectId='{projectId}'").Count() != 1)
                    return false;

                if (dh.RunQuery($"SELECT uName, ProjectId FROM [User_Project] " +
                    $"WHERE projectId='{projectId}' AND uName='{username}'").Count() == 1)
                    return false;

                dh.RunUpdateQuery($"INSERT INTO User_Project VALUES('{username}', '{projectId}');");
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