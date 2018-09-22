using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Task
    {
        private int taskId;
        private int workload;
        private int priority;
        private string title;
        private string description;
        private string status;
        private string projectName;
        private DateTime dueDate;
        private DateTime startDate;
        private DateTime completedDate;

        public int Workload {
            get {
                return this.workload;
            }

            set {
                this.workload = value;
            }
        }

        public int Priority {
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