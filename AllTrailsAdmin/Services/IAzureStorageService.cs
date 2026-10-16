namespace AllTrailsAdmin.Services
{
    public interface IAzureStorageService
    {
        // Service to upload file to zure Blob Storage
        // return: the URL of the uploaded file as a string
        public Task<string> UploadFileAsync(IFormFile file, string fileName);

        //
        // TO-DO: add a new function to delete a file to handle when a trail record is deleted
        //
    }
}
