using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whitebears.Models
{
    public class BlobViewModel
    {
        public string BlobContainerName { get; set; }
        public string StorageUri { get; set; }
        public string ActualFileName { get; set; }
        public string PrimaryUri { get; set; }
        public string FileExtension { get; set; }
        public string FileNameWithoutExt
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(ActualFileName);
            }
        }
        public string FileNameExtensionOnly
        {
            get
            {
                return System.IO.Path.GetExtension(ActualFileName).Substring(1);
            }
        }
    }
}   