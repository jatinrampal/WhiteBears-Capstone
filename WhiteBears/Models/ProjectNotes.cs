using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class ProjectNotes
    {
        private int projectNoteId;
        private int projectId;
        private string message;
        private DateTime sentDate;
        private string from;
        private string to;
        private DateTime completedDate;

        public int ProjectNoteId
        {
            get
            {
                return projectNoteId;
            }
            set
            {
                projectNoteId = value;
            }
        }

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

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        public DateTime SentDate
        {
            get
            {
                return sentDate;
            }
            set
            {
                sentDate = value;
            }
        }

        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }

        public string To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }

        public DateTime CompletedDate
        {
            get
            {
                return completedDate;
            }
            set
            {
                completedDate = value;
            }
        }
    }
}