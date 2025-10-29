using Microsoft.AspNetCore.Mvc;
using BibliotecaDigital.API.Services;

namespace BibliotecaDigital.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CapasController : ControllerBase
{
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public CapasController(IAzureBlobStorageService azureBlobStorageService)
    {
        _azureBlobStorageService = azureBlobStorageService;
    }

    /// <summary>
    /// Upload de capa de livro para Azure Blob Storage (SDK Oficial)
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> UploadCapa(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest(new { mensagem = "Arquivo inválido" });

        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

        if (!extensoesPermitidas.Contains(extensao))
            return BadRequest(new { mensagem = "Formato não permitido. Use JPG, PNG ou GIF" });

        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

        using var stream = arquivo.OpenReadStream();
        var url = await _azureBlobStorageService.UploadAsync(stream, nomeArquivo, arquivo.ContentType);

        return Ok(new 
        { 
            mensagem = "Capa enviada com sucesso",
            nomeArquivo,
            url,
            tamanho = arquivo.Length
        });
    }

    /// <summary>
    /// Download de capa de livro do Azure Blob Storage
    /// </summary>
    [HttpGet("download/{nomeArquivo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadCapa(string nomeArquivo)
    {
        try
        {
            var stream = await _azureBlobStorageService.DownloadAsync(nomeArquivo);
            var contentType = nomeArquivo.ToLower() switch
            {
                var n when n.EndsWith(".jpg") || n.EndsWith(".jpeg") => "image/jpeg",
                var n when n.EndsWith(".png") => "image/png",
                var n when n.EndsWith(".gif") => "image/gif",
                _ => "application/octet-stream"
            };

            return File(stream, contentType, nomeArquivo);
        }
        catch (Exception)
        {
            return NotFound(new { mensagem = "Arquivo não encontrado" });
        }
    }

    /// <summary>
    /// Lista todas as capas armazenadas no Azure Blob Storage
    /// </summary>
    [HttpGet("listar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> ListarCapas()
    {
        var arquivos = await _azureBlobStorageService.ListBlobsAsync();
        return Ok(new { total = arquivos.Count, arquivos });
    }

    /// <summary>
    /// Deleta uma capa do Azure Blob Storage
    /// </summary>
    [HttpDelete("{nomeArquivo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletarCapa(string nomeArquivo)
    {
        var deletado = await _azureBlobStorageService.DeleteAsync(nomeArquivo);

        if (!deletado)
            return NotFound(new { mensagem = "Arquivo não encontrado" });

        return Ok(new { mensagem = "Capa deletada com sucesso" });
    }
}
