using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Whitebears_BlobStorage.Repository;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

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
            //required a session variable for ProjectID and UploaderName
            bool isUploaded = repo.UploadBlob(uploadFileName);
            int isDBUpdated = UpdateDatabase("1", uploadFileName.FileName, "Jatin", System.IO.Path.GetExtension(uploadFileName.FileName));

            if (isUploaded == true && isDBUpdated == 0)
            {

                return RedirectToAction("Index");
            }
            return View();
        }

        public DateTime Timestamp { get; }

        public static int UpdateDatabase(string pID, string fileName, string uploaderName, string fileExt)
        {
            WhiteBears.DatabaseHelper dh = new WhiteBears.DatabaseHelper();
            return dh.RunInsertQuery($"INSERT INTO Document(ProjectID,FileName,Uploader,CreationTime,fileExtension) VALUES('{pID}','{fileName}','{uploaderName}','{DateTime.Now}','{fileExt}')");

        }
    }


}