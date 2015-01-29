using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace Fiveplus.Storage
{

   // Content Type : https://groups.google.com/forum/#!topic/google-appengine-stackoverflow/BhTPS5JPoJ8
    public static class StorageManager
    {
        public static string UploadImage(Stream fileStream, FileDesc fileDesc)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                        ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("media");

            // Create the container if it doesn't already exist.
            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            String uniqueBlobName;
           // String uniqueBlobName = String.Format(@"{0}/{1}/image_{2}{3}", fileDesc.UserName, fileDesc.GigId, fileDesc.Name + Guid.NewGuid(), Path.GetExtension(fileDesc.Name));
            if(!String.IsNullOrEmpty(fileDesc.GigId))
                uniqueBlobName = String.Format(@"{0}/{1}/{2}", fileDesc.UserName, fileDesc.GigId, fileDesc.Name);
            else
                uniqueBlobName = String.Format("{0}/{1}", fileDesc.UserName,fileDesc.Name);

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(uniqueBlobName);

            blockBlob.Properties.ContentType = fileDesc.ContentType;

            // Create or overwrite the "myblob" blob with contents from a local file.
            blockBlob.UploadFromStream(fileStream);
            return blockBlob.Uri.AbsoluteUri;
        }


    }
}
