using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whitebears_BlobStorage.Models;
using System.Threading.Tasks;

namespace Whitebears_BlobStorage.Repository
{
    public interface IBlobStorageRepository
    {
        IEnumerable<BlobViewModel> GetBlobs();
        bool DeleteBlob(string file, string fileExtension);
        bool UploadBlob(HttpPostedFileBase blobFile, int count);

        Task<bool> DownloadBlobAsync(string file, string fileExtension);
    }
}