using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LGH_Server
{
    class DocumentPusher
    {
        // https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=windows
        // 1 Navigate to the Azure portal.
        // 2 Locate your storage account.
        // 3 In the Settings section of the storage account overview, select Access keys.Your account access keys appear, as well as the complete connection string for each key.
        // 4 Find the Connection string value under key1, and click the Copy button to copy the connection string. You will add the connection string value to an environment variable in the next step.
        // For at lave en lokal environment variable med conenctiong stringen, gå til cmd og kør følgende command: setx storageconnectionstring "<yourconnectionstring>"
        // key1 Connection string: DefaultEndpointsProtocol=https;AccountName=littlegreenhelper;AccountKey=iNQUaRhmGwTyng4ujEbjiSCdi4USB91elR9Z6QAJZai2ajITqn2w+VEzxizQSJrhJA2L65NUtdGQD65R6ZLgZg==;EndpointSuffix=core.windows.net

        private static bool finishedPushingToServer = false;

        private static string myPath;
        private static string MyPath
        {
            get
            {
                if (myPath == null)
                {
                    string systemPath = AppDomain.CurrentDomain.BaseDirectory;

                    myPath = Path.Combine(systemPath, "AllBargains.txt");
                }
                return myPath;
            }
        }

        public void PushFilTilWebsite()
        {
            Console.WriteLine("PushFilTilWebsite Done.");
           ProcessAsync();
        }

        private static async Task ProcessAsync()
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
            string sourceFile = null;
            string destinationFile = null;

            // Retrieve the connection string for use with the application. The storage connection string is stored
            // in an environment variable on the machine running the application called storageconnectionstring.
            // If the environment variable is created after the application is launched in a console or with Visual
            // Studio, the shell needs to be closed and reloaded to take the environment variable into account.
            string storageConnectionString = "ur connectionstring here";

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    Console.WriteLine("PushFilTilWebsite.CreatingBlobClient. Done.");
                    // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
                    //cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("lghdocuments");
                    Console.WriteLine("PushFilTilWebsite.GetContainerReference. Done.");

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    Console.WriteLine("PushFilTilWebsite.Permissions. Done.");
                    await cloudBlobContainer.SetPermissionsAsync(permissions);
                    Console.WriteLine("PushFilTilWebsite.Permissions set. Done.");
                                                                       
                    // Create a file in your local MyDocuments folder to upload to a blob.
                    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string localFileName = GenerateTableName() + ".txt";
                    sourceFile = Path.Combine(localPath, localFileName);
                    // Write text to the file.
                    string contentToWrite = System.IO.File.ReadAllText(MyPath);

                    File.WriteAllText(sourceFile, contentToWrite);
                    Console.WriteLine("PushFilTilWebsite.WriteAllTExt. Done.");


                    // Get a reference to the blob address, then upload the file to the blob.
                    // Use the value of localFileName for the blob name.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
                    Console.WriteLine("PushFilTilWebsite.GetBlockBlobReference. Done.");
                    await cloudBlockBlob.UploadFromFileAsync(sourceFile);
                    Console.WriteLine("PushFilTilWebsite.Upload. Done.");

                    finishedPushingToServer = true;

                    // List the blobs in the container.
                    Console.WriteLine("Listing current blobs in container:");
                    BlobContinuationToken blobContinuationToken = null;
                    do
                    {
                        var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        blobContinuationToken = results.ContinuationToken;
                        foreach (IListBlobItem item in results.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (blobContinuationToken != null); // Loop while the continuation token is not null.
                    Console.WriteLine();

                    // Download the blob to a local file, using the reference created earlier. 
                    // Append the string "_DOWNLOADED" before the .txt extension so that you can see both files in MyDocuments.
                    destinationFile = sourceFile.Replace(".txt", "_DOWNLOADED.txt");
                    //Console.WriteLine("Downloading blob to {0}", destinationFile);
                    //Console.WriteLine();
                    await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);
                }
                catch (StorageException ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);
                }
                finally
                {

                }
            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named 'storageconnectionstring' with your storage " +
                    "connection string as a value.");
            }
            string test = "";
        }

        public bool IsFinishedPushingToServer()
        {
            return finishedPushingToServer;
        }

        private static string GenerateTableName()
        {
            string currentTime = DateTime.Now.ToString();

            currentTime = currentTime.Replace(" ", string.Empty).Replace("/", string.Empty);

            string tableName = "";

            for (int i = 0; i < currentTime.Length; i++)
            {
                // Day of month end.
                if (i == 2)
                {
                    tableName = tableName + ".";
                }

                // Month end.
                if (i == 4)
                {
                    tableName = tableName + ".";
                }

                // Year end.
                if (i == 8)
                {
                    tableName = tableName + ".";
                }

                tableName = tableName + currentTime[i];
            }

            return tableName;
        }

    }
}

