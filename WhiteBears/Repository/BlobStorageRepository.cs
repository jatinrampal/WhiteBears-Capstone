using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Whitebears.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace Whitebears.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private StorageCredentials _storageCredentials; //auth access to Azure Storage Account
        private CloudStorageAccount _cloudStorageAccount; //access to cloud storage account
        private CloudBlobClient _cloudBlobClient; //client logical rep
        private CloudBlobContainer _cloudBlobContainer; //blob container

        private string containerName = "test";
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads//");


        public BlobStorageRepository()
        {
            string accountName = "whitebearsdocs";
            string key = "yZyH4CDRTH+bVUdEpVxfA7E09jS21R2bfbctMqG8FIaW8A2vczumDft5Zove+CSv54XOTLcMs2FCSXkDSwUdlQ==";

            _storageCredentials = new StorageCredentials(accountName, key);
            _cloudStorageAccount = new CloudStorageAccount(_storageCredentials, true); //Uses HTTPs
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
        }

        public bool DeleteBlob(string file, string fileExtension)
        {
            //throw new NotImplementedException();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(file + "." + fileExtension);
            bool delete = blockBlob.DeleteIfExists();

            return delete;
        }

        public async Task<bool> DownloadBlobAsync(string file, string fileExtension)
        {
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(file + "." + fileExtension);

            using (var fileStream = System.IO.File.OpenWrite(downloadPath + file + "." + fileExtension))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
                return true;
            }
        }

        public MemoryStream GetBlobAsStream(string file, string fileExtension)
        {
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(file + "." + fileExtension);
            MemoryStream memStream = new MemoryStream();
            blockBlob.DownloadToStream(memStream);
            memStream.Position = 0;
            return memStream;
        }

        public long GetFileSize(string file, string fileExtension)
        {
            long fileSize;
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(file + "." + fileExtension);
            blockBlob.FetchAttributes();
            fileSize = blockBlob.Properties.Length;
            return fileSize / 1000;
        }

        public IEnumerable<BlobViewModel> GetBlobs()
        {
            var context = _cloudBlobContainer.ListBlobs().ToList();
            IEnumerable<BlobViewModel> vm = context.Select(x => new BlobViewModel
            {
                BlobContainerName = x.Container.Name,
                StorageUri = x.StorageUri.PrimaryUri.ToString(),
                PrimaryUri = x.StorageUri.PrimaryUri.ToString(),
                ActualFileName = x.Uri.AbsoluteUri.Substring(x.Uri.AbsoluteUri.LastIndexOf("/") + 1),
                FileExtension = System.IO.Path.GetExtension(x.Uri.AbsoluteUri.Substring(x.Uri.AbsoluteUri.LastIndexOf("/") + 1))

            }).ToList();

            return vm;
        }

        public bool UploadBlob(HttpPostedFileBase blobFile, int count)
        {
            if (blobFile == null)
            {
                return false;
            }

            string fileName = blobFile.FileName.ToString();

            int length = fileName.Length;
            int index = fileName.LastIndexOf(".");


            if (index > 0)
            {
                fileName = fileName.Substring(0, index);

            }


            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName + "_v" + (count + 1) + System.IO.Path.GetExtension(blobFile.FileName));

            using (var fileStream = blobFile.InputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }
            return true;
        }
    }
}