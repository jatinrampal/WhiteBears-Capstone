using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WhiteBears.Models
{
    public class User
    {
        [Required]
        private string firstName;

        [Required]
        private string lastName;

        [Required]
        private string username;

        [Required]
        private string password;

        private string fullName;

        private readonly string email;

        private string role;

        private PersonalNote[] personalNotes;

        private bool enabled;

        private int companyId;

        public int CompanyId
        {
            get
            {
                return companyId;
            }
            set
            {
                companyId = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public string FirstName {
            get {
                return firstName;
            }
            set
            {
                firstName = value; 
            }
        }

        public string Email {
            get {
                return email;
            }
        }

        public string Username {
            get {
                return username;
            }
            set
            {
                username = value; 
            }
        }

        public string LastName {
            get {
                return lastName;
            }
            set
            {
                lastName = value; 
            }
        }

        public string FullName {
            get {
                return fullName;
            }
            set
            {
                fullName = value; 
            }
        }

        public string Role {
            get {
                return role;
            }

            set {
                role = value;
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

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }


        //Testing constructor only
        public User(string firstName, string lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
            this. fullName = $"{firstName} {lastName}";
        }

        public User(string firstName, string lastName, string username, string email, string password, string role) {
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.email = email;
            this.password = password;
            this.fullName = $"{firstName} {lastName}";
            this.role = role;
        }

        public User(){}

        public string GetFullName() {
            return $"{firstName} {lastName}";
        }


        public PersonalNote[] LoadNotes(){
            //TODO

            return this.personalNotes;
        }
    }
}