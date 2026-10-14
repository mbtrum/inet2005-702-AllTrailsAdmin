using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AllTrailsAdmin.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly BlobServiceClient blobServiceClient;
        private string containerName;

        // Constructor
        public AzureStorageService(IConfiguration configuration)
        {
            // configuration is used to get appsettings, user-secrets, env vars, etc.
            _configuration = configuration;

            // get Azure Storage container name
            containerName = _configuration["AzureStorage:ContainerName"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage:ContainerName");

            // get Azure Storage connection string
            string connectionString = _configuration["AzureStorage:ConnectionString"] ?? throw new InvalidOperationException("Missing configuration: AzureStorage:ConnectionString");

            // initialize Azure blob client
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            // get client for containerName (e.g. "uploads")
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };

            // file I/O
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, options);
            }

            // return the URL for the saved file
            return blobClient.Uri.ToString();
        }
    }
}
