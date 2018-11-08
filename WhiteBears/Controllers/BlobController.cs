﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Whitebears.Repository;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Whitebears.Repository;
using System.Diagnostics;

namespace Whitebears.Controllers
{
    public class BlobController : Controller
    {

        private static SqlConnection con;
        private static SqlDataAdapter da;
        private static DataSet ds;
        private static DataRow[] dr;
        public string projectid { get; set; }
        public string role { get; set; }
        public string uname { get; set; }

        private readonly IBlobStorageRepository repo;
        public BlobController(IBlobStorageRepository _repo)
        {
            this.repo = _repo;

        }



        // GET: Blob
        public ActionResult Index()
        {
            var blobVM = repo.GetBlobs();
            return View(blobVM);
        }

        public JsonResult RemoveBlob(string file, string extension)
        {
            bool isDeleted = repo.DeleteBlob(file, extension);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DownloadBlob(string file, string extension)
        {
            bool isDownloaded = await repo.DownloadBlobAsync(file, extension);
            return RedirectToAction("Index");

        }

        public ActionResult UploadBlob()
        {
            // IF username session is null
            if (Session["username"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            role = Request["role"];
            projectid = Request["projectId"];
            uname = Session["username"].ToString();

            // Sends to the view through ViewBag 
            ViewBag.projectid = projectid;
            ViewBag.role = role;

            return View();
        }

        [HttpPost]
        public ActionResult UploadBlob(HttpPostedFileBase uploadFileName)
        {
            // Retrives from form 
            var role = Request["role"].ToString();
            var projectId = Request["projectId"];
            uname = Session["username"].ToString();

            //Testing
            Debug.WriteLine("Role: " + role);
            Debug.WriteLine("Project ID " + projectId);
            Debug.WriteLine("Username" + uname);


            string actualFileName = uploadFileName.FileName.ToString();
            int index = actualFileName.LastIndexOf(".");

            if (index > 0)
                actualFileName = actualFileName.Substring(0, index);

            //required a session variable for ProjectID and UploaderName
            /*string url = Request.Url.ToString();
            string[] strArray = url.Split('/');

            string projectid = strArray[6];*/

            //Check if a file exists with the same name
            int count = CheckDocumentVersionDB(projectId, actualFileName);

            //If filename doesn't exist INSERT and if it does UPDATE
            if (count == 0)
            {
                InsertDocumentDB(projectId, actualFileName , uname, System.IO.Path.GetExtension(uploadFileName.FileName));
            }
            else if (count > 0)
            {
                UpdateDocumentDB(projectId, actualFileName , actualFileName , uname, System.IO.Path.GetExtension(uploadFileName.FileName));
            }

            //Update the Database and put in the Document entry
            string documentID = CheckUploadDocumentID(actualFileName);


            //CloudBlobContainer.CreateIfNotExists Method

            string docName = actualFileName + "_v" + (count);


            bool isUploaded = repo.UploadBlob(uploadFileName, count);

            if (isUploaded == true)
            {
                //UploaderName Required Here
                UpdateDocumentVersionDB(documentID, count + 1, uname);
                UpdateDocumentRoleDB(role, documentID);
                return RedirectToAction("Index");
            }
            return View();
        }

        public static int InsertDocumentDB(string pID, string fileName, string uploaderName, string fileExt)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunInsertQuery($"INSERT INTO Document(ProjectID,FileName,Uploader,CreationTime,fileExtension) VALUES('{pID}','{fileName}','{uploaderName}','{DateTime.Now}','{fileExt}')");
        }

        public static int UpdateDocumentDB(string pID, string fileName, string updatedFileName, string uploaderName, string fileExt)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunUpdateQuery($"UPDATE DOCUMENT SET CreationTime='{DateTime.Now}', FileName='{updatedFileName}' WHERE FileName = '{fileName}'");
        }

        public static int CheckDocumentVersionDB(string pId, string fileName)
        {
            int count;
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            count = dh.RunQuery($"SELECT * FROM DocumentVersion JOIN Document ON Document.DocumentID = DocumentVersion.DocumentID  WHERE FileName = '{fileName}' ").Count();
            return count;
        }

        public static string CheckUploadDocumentID(string fileName)
        {
            string id;
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            DataRow[] a = dh.RunSelectQuery($"SELECT DocumentId FROM Document WHERE FileName = '{fileName}'");
            return a[0][0].ToString();
        }

        public static int UpdateDocumentVersionDB(string documentId, int version, string uploaderName)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunInsertQuery($"INSERT INTO DocumentVersion VALUES('{version}','{documentId.ToString()}','{DateTime.Now}','{uploaderName}')");
        }

        public static int UpdateDocumentRoleDB(string role, string documentId)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunInsertQuery($"INSERT INTO DocumentRole VALUES('{role}','{documentId}', 1)");
        }
    }


}