using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class Task
    {
        private int taskId;
        private int workload;
        private string priority;
        private string title;
        private string description;
        private string status;
        private string projectName;
        private DateTime dueDate;
        private DateTime startDate;
        private DateTime completedDate;
        private string projectId; 
        


        public string ProjectId
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

        public Boolean IsCompleted{
            get{
                return !completedDate.ToString("MM/dd/yyyy").Contains("0001");
            }
        }

        public int TaskId{
            get{
                return taskId;
            }

            set{
                taskId = value;
            }
        }

        public DateTime CompletedDate{
            get{
                return completedDate;
            }

            set{
                completedDate = value;
            }
        }


        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
            }
        }

        public string Description{
            get{
                return description;
            }

            set{
                description = value;
            }
        }


        public int Workload {
            get {
                return this.workload;
            }

            set {
                this.workload = value;
            }
        }

        public string Priority {
            get {
                return this.priority;
            }

            set {
                this.priority = value;
            }
        }

        public string Title {
            get {
                return this.title;
            }

            set {
                this.title = value;
            }
        }

        public DateTime DueDate {
            get {
                return this.dueDate;
            }

            set {
                dueDate = value;
            }
        }

        public string Status {
            get {
                return this.status;
            }

            set {
                status = value;
            }
        }

        public string ProjectName {
            get {
                return this.projectName;
            }

            set {
                projectName = value;
            }
        }
    }
}