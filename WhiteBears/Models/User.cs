using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class User
    {
        public int uName { get; set; }
        public int companyId { get; set; }
        public String password { get; set; }

        public String firstName { get; set; }

        public String lastName { get; set; }
    }
}