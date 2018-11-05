using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class ElementJSON
    {
        public int id { get; set; }
        public int order { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public ElementJSON[] elements { get; set; }
    }
}