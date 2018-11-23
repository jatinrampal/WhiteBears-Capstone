using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class ProjectPageViewModel
    {
        public ProjectPageViewModel()
        {
            this.User = new User();
            this.Project = new Project();
            this.ProjectNotes = new ProjectNotes();
            this.Task = new Task();
            this.document = new Document();
            this.documentRole = new DocumentRole();

        }

        public string[] GetUsers {get; set;}

        public User User { get; set; }

        public Project Project { get; set; }

        public ProjectNotes ProjectNotes { get; set; }

        public Task Task { get; set; }
        public Document document { get; set; }

        public DocumentRole documentRole { get; set; }

    
    }
}