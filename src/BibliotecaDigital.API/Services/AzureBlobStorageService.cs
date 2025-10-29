using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace BibliotecaDigital.API.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly BlobServiceClient? _blobServiceClient;
    private readonly string _containerName;
    private bool _containerCreated = false;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:ConnectionString"];
        _containerName = configuration["AzureStorage:ContainerName"] ?? "capas-livros";
        
        // Verifica se o Azure Storage está configurado
        if (string.IsNullOrEmpty(connectionString) || connectionString == "DISABLED")
        {
            _blobServiceClient = null;
            return;
        }

        try
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            // NÃO cria o container aqui - será criado sob demanda
        }
        catch (Exception)
        {
            _blobServiceClient = null;
        }
    }

    private void EnsureContainerExists()
    {
        if (_blobServiceClient == null)
            throw new InvalidOperationException("Azure Blob Storage não está disponível. Configure a connection string ou inicie o Azure Storage Emulator (Azurite).");

        if (_containerCreated)
            return;

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            _containerCreated = true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Não foi possível conectar ao Azure Storage. Certifique-se de que o Azurite está rodando ou configure uma connection string válida. Erro: {ex.Message}", ex);
        }
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        EnsureContainerExists();

        var containerClient = _blobServiceClient!.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(fileStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string blobName)
    {
        EnsureContainerExists();

        var containerClient = _blobServiceClient!.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string blobName)
    {
        EnsureContainerExists();

        var containerClient = _blobServiceClient!.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> ListBlobsAsync()
    {
        EnsureContainerExists();

        var containerClient = _blobServiceClient!.GetBlobContainerClient(_containerName);
        var blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }
}
