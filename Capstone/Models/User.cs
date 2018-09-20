using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class User
    {
        [Required]
        private readonly int userId;

        [Required]
        private readonly string firstName;

        [Required]
        private readonly string lastName;

        [Required]
        private readonly string username;

        [Required]
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