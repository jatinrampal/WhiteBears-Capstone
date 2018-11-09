using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class DocumentRole
    {
        private string roleName;
        private int documentId;
        private bool writeAccess;
      
        public string RoleName
        {
            get
            {
                return roleName; 
            }
            set
            {
                roleName = value; 
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

        public bool WriteAccess
        {
            get
            {
                return writeAccess;
            }
            set
            {
                writeAccess = value;
            }
        }

    }
}