using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class DashboardModel
    {
        public Project[] Projects { get; set; }
        public User CurrentUser { get; set; }

        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrDate { get; set; }
    }
}