using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Project
    {
        private int projectId;
        private string title, description, scopeStatement;
        private DateTime startDate, dueDate, completionDate;

        public void LoadProject(){
            //TODO
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