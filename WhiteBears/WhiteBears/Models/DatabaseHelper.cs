using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBears.Models
{
    public class DatabaseHelper
    {
        private readonly SqlConnection conn;
        private SqlDataAdapter da;
        private DataSet ds;
        private DataRow[] dr;

        public DatabaseHelper()
        {
            conn = new SqlConnection(@"Data Source=whitebears-server.database.windows.net;Initial Catalog=whitebears-db;Persist Security Info=True;User ID=sysdba;Password=j3.'(Ge=");
        }

        public DataRow[] RunQuery(string sQuery)
        {
            using (conn)
            {
                da = new SqlDataAdapter(sQuery, conn);
                ds = new DataSet();
                da.Fill(ds);

                dr = ds.Tables[0].Select();
            }

            return dr;
        }
    }
}