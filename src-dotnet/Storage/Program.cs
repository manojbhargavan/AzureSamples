using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class Program
{
    private const string blobServiceEndpoint = "https://mediastormanoj.blob.core.windows.net/";
    private const string storageAccountName = "mediastormanoj";
    private const string storageAccountKey = "r251GcxnCMbttFCscdBuyGAgcQ2M6Nx1ugf0nUnqj1SWigg3JuVbAfJENuKkRl3e9YfW6IUq4yfo+AStDjGN1A==";

    public static async Task Main(string[] args)
    {
        StorageSharedKeyCredential accountCred = new(storageAccountName, storageAccountKey);
        BlobServiceClient serviceClient = new(new Uri(blobServiceEndpoint), accountCred);
        AccountInfo info = await serviceClient.GetAccountInfoAsync();

        await Console.Out.WriteLineAsync("Connected to Azure Storage Account");
        await Console.Out.WriteLineAsync($"Account Name: {storageAccountName}, Account Kind: {info?.AccountKind}, Sku: {info?.SkuName}");
        await EnumerateContainerAsync(serviceClient);

        string existingContainerName = "raster-graphics";
        await EnumerateBlobsAsync(serviceClient, existingContainerName);

        string newContainerName = "vector-graphics";
        var containerClient = await GetContainerAsync(serviceClient, newContainerName);

        string uploadedBlobName = "graph.svg";
        var blobClient = await GetBlobAsync(containerClient, uploadedBlobName);
    }

    private static async Task EnumerateContainerAsync(BlobServiceClient client)
    {
        await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
        {
            await Console.Out.WriteLineAsync($"Container: {container.Name}");
        }
    }

    private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
    {
        var container = client.GetBlobContainerClient(containerName);
        await Console.Out.WriteLineAsync($"Searching Container: {container.Name}");

        await foreach (BlobItem bItem in container.GetBlobsAsync())
        {
            await Console.Out.WriteLineAsync($"Existing Blob: {bItem.Name}");
        }
    }

    private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
    {
        BlobContainerClient container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        await Console.Out.WriteLineAsync($"New Container: {container.Name}");

        return container;
    }

    private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
    {
        var blob = client.GetBlobClient(blobName);
        bool blobExists = await blob.ExistsAsync();

        if (blobExists)
        {
            await Console.Out.WriteLineAsync($"Blob Found: {blob.Uri}");
        }
        else
        {
            await Console.Out.WriteLineAsync($"Blob Not Found: {blobName}");
        }

        return blob;
    }

}
