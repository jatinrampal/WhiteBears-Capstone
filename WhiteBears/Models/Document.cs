using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class Document
    {
        private int projectId;
        private int documentId; 
        private string fileName;
        private string uploader;
        private DateTime creationTime;
        private string fileExtension;
        public DocumentVersionsModel docVersion;
     

        public int ProjectId
        {
            get
            {
                return projectId; 
            }
            set
            {
                projectId = value; 
            }
        }

        public int DocumentId
        {
            get
            {
                return documentId; 
            }
            set
            {
                documentId = value; 
            }
        }

        public string FileName
        {
            get
            {
                return fileName; 
            }

            set
            {
                fileName = value; 
            }
        }

        public string Uploader
        {
            get
            {
                return uploader; 
            }
            set
            {
                uploader = value;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return creationTime; 
            }
            set
            {
                creationTime = value;
            }
        }

        public string FileExtension
        {
            get
            {
                return fileExtension; 
            }
            set
            {
                fileExtension = value; 
            }
        }
        public DocumentVersionsModel DocVersion
        {
            get
            {
                return docVersion;
            }
            set
            {
                docVersion = value;
            }
        }
    }
}