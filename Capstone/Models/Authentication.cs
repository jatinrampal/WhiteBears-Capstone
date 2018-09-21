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
    }
}