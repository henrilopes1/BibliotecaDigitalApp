namespace BibliotecaDigital.API.Services;

public interface IAzureBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream> DownloadAsync(string blobName);
    Task<bool> DeleteAsync(string blobName);
    Task<List<string>> ListBlobsAsync();
}
