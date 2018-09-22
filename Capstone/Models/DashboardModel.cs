using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class DashboardModel
    {
        public Project[] Projects { get; set; }
        public User CurrentUser { get; set; }

        public DateTime CurrDate { get; set; }
    }
}