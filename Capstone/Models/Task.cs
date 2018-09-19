using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Task
    {
        private int taskId, workLoad, priority;
        private string title, description;
        private DateTime dueDate, startDate, completedDate;
    }
}