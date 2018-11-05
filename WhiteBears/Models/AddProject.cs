using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class AddProject
    {
        public string Title { get; set; }
        public string[] Description { get; set; }
        public string[] ScopeStatement { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}