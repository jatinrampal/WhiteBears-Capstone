using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class Project
    {
        private int projectId;
        private string title, description, scopeStatement;
        private DateTime startDate, dueDate, completionDate;
        private Task[] tasks;

        public int ProjectId {
            get {
                return projectId;
            }

            set {
                projectId = value;
            }
        }

        public string Title {
            get {
                return title;
            }

            set {
                title = value;
            }
        }

        public string Description {
            get {
                return description;
            }

            set {
                description = value;
            }
        }

        public string ScopeStatement {
            get {
                return scopeStatement;
            }

            set {
                scopeStatement = value;
            }
        }

        public Task[] Tasks {
            get {
                return tasks;
            }

            set {
                tasks = value;
            }
        }

        public DateTime StartDate {
            get {
                return startDate;
            }

            set {
                startDate = value;
            }
        }

        public DateTime DueDate {
            get {
                return dueDate;
            }

            set {
                dueDate = value;
            }
        }

        public DateTime CompletionDate {
            get {
                return completionDate;
            }

            set {
                completionDate = value;
            }
        }

        public void LoadProject()
        {

        }

        public void GetNextMilestone()
        {
            //TODO
        }

        public void NextMilestoneCompletion()
        {
            //TODO
        }

        public void TasksCompletion()
        {
            //TODO
        }

        public void WorkCompletion()
        {
            //TODO
        }
    }
}