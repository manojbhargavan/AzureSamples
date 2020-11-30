using AzureStorageWrapper.Helper;
using AzureStorageWrapper.Models;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureStorageWrapper
{
    class Program
    {
        static int Main(string[] args)
        {
            //Container Helper
            var containerHelper = new ContainerHelper();
            var tableHelper = new TableHelper();

            while (true)
            {
                string option;

                Console.WriteLine(
                    "\n---------\nOptions\n---------\n" +
                    "1 --> List Container\n" +
                    "2 --> Create Container\n" +
                    "3 --> Upload Blob in an existing container\n" +
                    "4 --> Upload Folder contents as Blob(s) in an existing container\n" +
                    "5 --> List Blobs in a container\n" +
                    "6 --> Download a Blob from a container\n" +
                    "7 --> Create Cloud Table\n" +
                    "8 --> Create Entity in Cloud Table\n" +
                    "Other --> Quit\n---------");


                Console.Write("Enter your choice: ");
                option = Console.ReadLine();
                Console.WriteLine();


                switch (option.Trim())
                {
                    case "1":
                        ListContainers(containerHelper);
                        break;
                    case "2":
                        string containerName;
                        containerName = GetContainerName();
                        CreateContainer(containerHelper, containerName);
                        break;
                    case "3":
                        ListContainers(containerHelper);
                        containerName = GetContainerName();
                        string localfile = GetFileName();
                        CreateBlob(containerHelper, containerName, localfile);
                        break;
                    case "4":
                        ListContainers(containerHelper);
                        containerName = GetContainerName();
                        localfile = GetFileName(true);
                        if (Directory.Exists(localfile))
                        {
                            var files = Directory.GetFiles(localfile);
                            foreach (var file in files)
                            {
                                Console.WriteLine($"Uploading {file}");
                                CreateBlob(containerHelper, containerName, file);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No such folder");
                        }
                        break;
                    case "5":
                        ListBlobs(containerHelper);
                        break;
                    case "6":
                        ListContainers(containerHelper);
                        containerName = GetContainerName();
                        localfile = GetFileName(true);
                        if (!Directory.Exists(localfile))
                        {
                            Directory.CreateDirectory(localfile);
                        }
                        DownloadContainerBlobs(containerHelper, containerName, localfile);
                        break;
                    case "7":
                        Console.Write("Enter table name: ");
                        string tableName = Console.ReadLine();
                        var cloudTable = tableHelper.CreateTable(tableName);
                        Console.WriteLine($"Table Create --> {cloudTable.Uri}");
                        break;
                    case "8":
                        Console.Write("Enter table name: ");
                        tableName = Console.ReadLine();
                        cloudTable = tableHelper.CreateTable(tableName);
                        Console.WriteLine($"Table Create --> {cloudTable.Uri}");
                        Console.Write("City: ");
                        string city = Console.ReadLine();
                        Console.Write("First Name: ");
                        string fName = Console.ReadLine();
                        Console.Write("Last Name: ");
                        string lName = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        //Console.Write("Phone Number: ");
                        //string phoneNumber = Console.ReadLine();
                        string id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                        CustomerEntity cust = new CustomerEntity(city, id)
                        {
                            Email = email,
                            FirstName = fName,
                            LastName = lName
                        };
                        var result = tableHelper.InsertEntity(cloudTable, cust);
                        Console.WriteLine(result.ToString());
                        break;
                    default: return 0;
                }

            }
        }

        private static void DownloadContainerBlobs(ContainerHelper containerHelper, string containerName, string localFolder)
        {
            if (containerHelper.ContainerExists(containerName))
            {
                var blobs = containerHelper.GetBlobs(containerName);
                foreach (var blob in blobs)
                {
                    Console.WriteLine($"Downloading {blob.Name}");
                    containerHelper.DownloadBlob(containerName, blob.Name, Path.Join(localFolder, blob.Name));
                    Console.WriteLine($"{blob.Name} downloaded to {Path.Join(localFolder, blob.Name)}");
                }
            }
        }

        private static string GetContainerName()
        {
            string containerName;
            Console.Write("Enter container name: ");
            containerName = Console.ReadLine();
            return containerName;
        }

        private static void ListBlobs(ContainerHelper containerHelper)
        {
            string containerName;
            ListContainers(containerHelper);
            Console.Write("Enter the container name you want to list the blob in: ");
            containerName = Console.ReadLine();
            if (containerHelper.ContainerExists(containerName))
            {
                var blobs = containerHelper.GetBlobs(containerName);
                Console.WriteLine();
                if (blobs?.Count() == 0)
                {
                    Console.WriteLine("Container is empty");
                }
                else
                {
                    foreach (var blob in blobs)
                    {
                        Console.WriteLine($"Name: {blob.Name}, Type: {blob.Properties.BlobType}, Tier: {blob.Properties.AccessTier}, Size: {blob.Properties.ContentLength} Bytes");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No container is named {containerName}");
            }
        }

        private static void CreateBlob(ContainerHelper containerHelper, string containerName, string localfileName)
        {
            if (containerHelper.ContainerExists(containerName))
            {
                if (File.Exists(localfileName))
                {
                    var blobInfo = containerHelper.CreateBlob(containerName, new FileInfo(localfileName));
                    Console.WriteLine(blobInfo?.ETag);
                }
                else
                {
                    Console.WriteLine("No such file found");
                }
            }
            else
            {
                Console.WriteLine($"No container is named {containerName}");
            }

        }

        private static string GetFileName(bool folder = false)
        {
            if (folder)
                Console.Write("Enter Local Folder name: ");
            else
                Console.Write("Enter Local File full name: ");

            string localfile = Console.ReadLine();
            return localfile;
        }

        private static void CreateContainer(ContainerHelper containerHelper, string containerName)
        {
            if (!string.IsNullOrWhiteSpace(containerName))
            {
                if (containerHelper.ContainerExists(containerName))
                {
                    Console.WriteLine("A container is already present with same name, see the full list of containers in the storage account");
                    ListContainers(containerHelper);
                }
                else
                {
                    containerHelper.CreateContainer(containerName).Wait();
                }
            }
            else
            {
                Console.WriteLine($"Name is required");
            }
        }

        private static void ListContainers(ContainerHelper containerHelper)
        {
            foreach (var container in containerHelper.Containers)
            {
                Console.WriteLine($"Account Name: {container.Value.AccountName}, Container Name: {container.Value.Name}");
            }
        }
    }
}
