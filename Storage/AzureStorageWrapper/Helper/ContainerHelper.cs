using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureStorageWrapper.Helper
{
    public class ContainerHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        public List<KeyValuePair<string, BlobContainerClient>> Containers { get; } 
            = new List<KeyValuePair<string, BlobContainerClient>>();
        public ContainerHelper()
        {
            _blobServiceClient = new BlobServiceClient(Constants.StorageAccountConnectionString);
            Containers = new List<KeyValuePair<string, BlobContainerClient>>();
            PopulateContainerList();
        }

        public async Task<bool> CreateContainer(string containerName)
        {
            if (!Containers.Any(kv => kv.Key.Equals(containerName, StringComparison.InvariantCultureIgnoreCase)))
            {
                var response = await _blobServiceClient.CreateBlobContainerAsync(blobContainerName: containerName);
                Containers.Add(new KeyValuePair<string, BlobContainerClient>(containerName, response.Value));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContainerExists(string containerName)
        {
            return Containers.Any(kv => kv.Key.Equals(containerName, StringComparison.InvariantCultureIgnoreCase));
        }

        public BlobContentInfo CreateBlob(string containerName, FileInfo blobFileInfo)
        {
            try
            {
                var containerToUpload = Containers.FirstOrDefault(kv => kv.Key.Equals(containerName, StringComparison.InvariantCultureIgnoreCase));
                return containerToUpload.Value.UploadBlob(blobFileInfo.Name, File.OpenRead(blobFileInfo.FullName));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public List<BlobItem> GetBlobs(string containerName)
        {
            var containerToList = Containers.FirstOrDefault(kv => kv.Key.Equals(containerName, StringComparison.InvariantCultureIgnoreCase));
            var blobs = containerToList.Value.GetBlobs();
            return blobs.ToList();
        }

        public void DownloadBlob(string containerName, string blobName, string downloadPath)
        {
            var containerToList = Containers.FirstOrDefault(kv => kv.Key.Equals(containerName, StringComparison.InvariantCultureIgnoreCase));
            var blobs = containerToList.Value.GetBlockBlobClient(blobName);
            var download = blobs.Download();
            using (FileStream file = File.OpenWrite(downloadPath))
            {
                download.Value.Content.CopyTo(file);
            }
        }

        private void PopulateContainerList()
        {
            var containerList = _blobServiceClient.GetBlobContainers();
            foreach(var container in containerList)
            {
                Containers.Add(new KeyValuePair<string, BlobContainerClient>(container.Name, 
                    new BlobContainerClient(Constants.StorageAccountConnectionString, container.Name)));
            }
        }
    }
}
