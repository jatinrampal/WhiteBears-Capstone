using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Whitebears_BlobStorage.Repository;
using System.Threading.Tasks;

namespace Whitebears_BlobStorage.Controllers
{
    public class BlobController : Controller
    {
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
            bool isUploaded = repo.UploadBlob(uploadFileName);
            if(isUploaded == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }

    
}