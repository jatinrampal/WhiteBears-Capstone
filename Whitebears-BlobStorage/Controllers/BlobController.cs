using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Whitebears_BlobStorage.Repository;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Whitebears_BlobStorage.Controllers
{
    public class BlobController : Controller
    {

        private static SqlConnection con;
        private static SqlDataAdapter da;
        private static DataSet ds;
        private static DataRow[] dr;

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
        [HttpGet]
        public ActionResult UploadBlob()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadBlob(HttpPostedFileBase uploadFileName)
        {
            string actualFileName = uploadFileName.FileName.ToString();
            int index = actualFileName.LastIndexOf(".");

            if (index > 0)
                actualFileName = actualFileName.Substring(0, index);

            //required a session variable for ProjectID and UploaderName

            

            //Check if a file exists with the same name
            int count = CheckDocumentVersionDB("1", actualFileName);

            //If filename doesn't exist INSERT and if it does UPDATE
            if (count==0)
            {
                InsertDocumentDB("1", actualFileName /*+ "_v" + (count + 1)*/, "Jatin", System.IO.Path.GetExtension(uploadFileName.FileName));
            }
            else if(count>0)
            {
                UpdateDocumentDB("1", actualFileName /*+ "_v" + (count)*/, actualFileName /*+ "_v" + (count + 1)*/, "Jatin", System.IO.Path.GetExtension(uploadFileName.FileName));
            }

            //Update the Database and put in the Document entry
            string documentID = CheckUploadDocumentID(actualFileName /*+ "_v" + (count + 1)*/);

            /*var type = typeof(HttpPostedFileBase);
            var property = type.GetProperty("FileName");

            var backingField = type
                            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                            .FirstOrDefault(field =>
                            field.Attributes.HasFlag(FieldAttributes.Private) &&
                            field.Attributes.HasFlag(FieldAttributes.InitOnly) &&
                            field.CustomAttributes.Any(attr => attr.AttributeType == typeof(CompilerGeneratedAttribute)) &&
                            (field.DeclaringType == property.DeclaringType) &&
                            field.FieldType.IsAssignableFrom(property.PropertyType) &&
                            field.Name.StartsWith("<" + property.Name + ">")
                            ); */

            //backingField.SetValue(uploadFileName.FileName, actualFileName + "_v" + (count + 1));
            //CloudBlobContainer.CreateIfNotExists Method

            string docName = actualFileName + "_v" + (count);
            

            bool isUploaded = repo.UploadBlob(uploadFileName, count);

            if (isUploaded == true )
            {
                //UploaderName Required Here
                UpdateDocumentVersionDB(documentID, count+1, "Jatin");
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
            DataRow[] a= dh.RunSelectQuery($"SELECT DocumentId FROM Document WHERE FileName = '{fileName}'");
            return a[0][0].ToString();

        }


        public static int UpdateDocumentVersionDB(string documentId, int version, string uploaderName)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunInsertQuery($"INSERT INTO DocumentVersion VALUES('{version}','{documentId.ToString()}','{DateTime.Now}','{uploaderName}')");
        }
    }


}