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

        private readonly string fullName;

        private PersonalNote[] personalNotes;

        public string FirstName {
            get {
                return firstName;
            }
        }

        public string LastName {
            get {
                return lastName;
            }
        }

        public string FullName {
            get {
                return fullName;
            }
        }

        public PersonalNote[] PersonalNotes {
            get {
                return personalNotes;
            }

            set {
                personalNotes = value;
            }
        }


        //Testing constructor only
        public User(string firstName, string lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
            this. fullName = $"{firstName} {lastName}";
        }

        public User(int userId, string firstName, string lastName, string username, string password) {
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.password = password;
            this.fullName = $"{firstName} {lastName}";
        }

        public string GetFullName() {
            return $"{firstName} {lastName}";
        }


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