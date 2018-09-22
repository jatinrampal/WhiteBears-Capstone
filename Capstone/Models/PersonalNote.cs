using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class PersonalNote
    {
        private int personalNoteId;
        private string information;
        private DateTime timeStamp;

        public string Information {
            get {
                return information;
            }

            set {
                information = value;
            }
        }

        public DateTime TimeStamp {
            get {
                return timeStamp;
            }

            set {
                timeStamp = value;
            }
        }

        public bool AddNote(){
            //TODO

            return false;
        }

        public bool ModifyNote(string information){
            this.information = information;
            timeStamp = new DateTime();

            return true;
        }

        public bool DeleteNote(){
            //TODO

            return false;
        }

        public PersonalNote DisplayNote(){
            return this;
        }
    }
}