using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SautinSoft.Document;
using System.IO;

namespace WhiteBears.Models
{
    public class DocumentVersionsModel
    {
        public List<SelectableVersions> docList { get; set; }
    public DocumentVersionsModel()
        {
            docList = new List<SelectableVersions>();
        }
    }
    public class SelectableVersions
    {
        public int version { get; set; }
        public string timeStamp { get; set; }
    }
}