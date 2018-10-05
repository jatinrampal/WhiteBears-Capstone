using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class User
    {
        private readonly int userId;
        private readonly string firstName;
        private readonly string lastName;
        private readonly string username;
        private readonly string password;
        private PersonalNote[] personalNotes;


        public Task[] LoadTasks(int projectId, int userId, string role){
            //TODO
            return null;
        }

        public PersonalNote[] LoadNotes(){
            //TODO

            return this.personalNotes;
        }
    }
}