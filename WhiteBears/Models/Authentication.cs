using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBears
{
    class Authentication
    {
        public static bool VerifyCredentials(string sUser, string sPassword){
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{sUser}' AND password = '{sPassword}'").Count() == 1;
        }

        public static bool VerifyIfEnabled(string sUser) {
            DatabaseHelper dh = new DatabaseHelper();
            DataRow[] drs = dh.RunQuery($"SELECT enabled FROM [User] WHERE uName = '{sUser}'");

            return Convert.ToBoolean(drs[0]["enabled".ToString()]);
        }

        public static bool VerifyIfPartOfProject(string username, int projectId) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM User_Project WHERE uName = '{username}' AND projectId = '{projectId}'").Count() == 1;
        }

        public static bool VerifyIfAdmin(string sUser) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT role FROM [User] WHERE uName = '{sUser}'")[0]["role"].ToString() == "Admin";
        }

        public static bool VerifyIfProjectManager(string sUser) {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT role FROM [User] WHERE uName = '{sUser}'")[0]["role"].ToString() == "Project Manager";
        }
    }
}