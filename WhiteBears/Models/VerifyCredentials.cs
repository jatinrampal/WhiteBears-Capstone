using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WhiteBears.Models
{
    public class VerifyCredentials
    {
        private static SqlConnection con;
        private static SqlDataAdapter da;
        private static DataSet ds;
        private static DataRow[] dr; 

        public static bool VerifyLogin(string sUser, string sPassword)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{sUser}' AND password = '{sPassword}'").Count() == 1;
        }
    }
}