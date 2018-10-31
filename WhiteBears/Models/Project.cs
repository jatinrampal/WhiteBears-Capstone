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

        public Task[] Tasks {
            get {
                return tasks;
            }

            set {
                tasks = value;
            }
        }


        public string Description { get; internal set; }
        public string ScopeStatement { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public DateTime DueDate { get; internal set; }
        public DateTime CompletionDate { get; internal set; }



        public void LoadProject(){
            
        }

        /*
        public Document[] LoadDocuments(){

        }git
        */

        public void GetNextMilestone(){
            //TODO
        }

        public void NextMilestoneCompletion(){
            //TODO
        }

        public void TasksCompletion(){
            //TODO
        }

        public void WorkCompletion(){
            //TODO
        }
    }
}