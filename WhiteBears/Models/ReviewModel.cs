using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SautinSoft.Document;
using System.IO;
using WhiteBears;

namespace WhiteBears.Models
{
    public class ReviewModel
    {
        public string modifiedBy1 { get; set; }
        public string timeStamp1 { get; set; }
        public string modifiedBy2 { get; set; }
        public string timeStamp2 { get; set; }

        public string Doc1 { get; set; }
        public string Doc2 { get; set; }
        public long fileSize1 { get; set; }
        public long fileSize2 { get; set; }
        
    }
}