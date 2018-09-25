using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using WhiteBears;

namespace Capstone.Models
{
    public class DashboardModel
    {
        public Project[] Projects { get; set; }
        public User CurrentUser { get; set; }

        [DisplayFormat(DataFormatString = "{MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CurrDate { get; set; }

        public DataRow[] GetUser(String username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM [User] WHERE uName = '{username}';");
        }

        public DataRow[] GetUserRole(String username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT role FROM [User_Project] WHERE uName = '{username}';");
        }

        public DataRow[] GetProjects(string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM Project p " +
                $"INNER JOIN User_Project up ON p.projectId = up.projectId " +
                $"WHERE up.uName = '{username}';");
        }

        public DataRow[] GetProject(string projectId, string username)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM Project p " +
                $"INNER JOIN User_Project up ON p.projectId = up.projectId " +
                $"WHERE up.uName = '{username}';");
        }

        public DataRow[] GetTasks(string sUser, int projectId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM Task t " +
                $"JOIN Project p on t.projectId = p.projectId " +
                $"WHERE p.projectId = '{projectId}';");
        }

        public DataRow[] GetPersonalNote(string uName)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunQuery($"SELECT * FROM PersonalNote WHERE PersonalNote.uName = '{uName}';");
        }

        public int AddPersonalNote(string uName, string note)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunUpdateQuery($"INSERT INTO PersonalNote VALUES('{uName}', '{note}', '{DateTime.Now}');");
        }

        public int DeletePersonalNote(int noteId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            //DELETE FROM PersonalNote WHERE PersonalNote.uName = 'Kish' AND PersonalNote.noteId = 2;
            return dh.RunUpdateQuery($"DELETE FROM PersonalNote WHERE PersonalNote.noteId = '{noteId}';");
        }

        public int UpdatePersonalNote(string note, int noteId)
        {
            DatabaseHelper dh = new DatabaseHelper();
            return dh.RunUpdateQuery($"UPDATE PersonalNote SET note='{note}' WHERE PersonalNote.noteId = '{noteId}';");
        }
    }
}