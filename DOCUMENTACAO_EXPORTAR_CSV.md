# Endpoint ExportarCSV - Documentação

## Funcionalidade
O endpoint `ExportarCSV` foi criado para exportar todos os livros da base de dados em formato CSV.

## Detalhes do Endpoint

**Rota:** `GET /api/livros/exportar-csv`
**Content-Type:** `text/csv`
**Nome do arquivo:** `livros.csv`

## Formato do CSV

O arquivo CSV gerado contém os seguintes cabeçalhos:
- `Id`: Identificador único do livro
- `Titulo`: Título do livro (entre aspas para tratar vírgulas)
- `AnoPublicacao`: Ano de publicação (pode estar vazio)

## Exemplo de Saída

```csv
Id,Titulo,AnoPublicacao
1,"Dom Casmurro",1899
2,"O Cortiço",1890
3,"Python Programming Guide",2023
4,"Clean Code: A Handbook of Agile Software Craftsmanship",2008
```

## Características Técnicas

### ✅ **Implementado com:**
- **StreamWriter**: Para construção eficiente da string CSV
- **MemoryStream**: Para manipulação em memória
- **Encoding UTF-8**: Para suporte a caracteres especiais
- **Escape de CSV**: Tratamento de aspas duplas e quebras de linha
- **Tratamento de Erros**: Retorna status 500 em caso de erro

### 🛡️ **Tratamento de Dados:**
- Títulos são escapados para CSV (aspas duplas são duplicadas)
- Quebras de linha são removidas
- Valores nulos de ano são tratados como string vazia
- Headers HTTP corretos para download de arquivo

### 📝 **Código Principal:**

```csharp
[HttpGet("exportar-csv")]
public async Task<IActionResult> ExportarCSV()
{
    try
    {
        var livros = await _livroRepository.GetAllAsync();

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

        await streamWriter.WriteLineAsync("Id,Titulo,AnoPublicacao");

        foreach (var livro in livros)
        {
            var linha = $"{livro.Id},\"{EscaparCsv(livro.Titulo)}\",{livro.AnoPublicacao?.ToString() ?? ""}";
            await streamWriter.WriteLineAsync(linha);
        }

        await streamWriter.FlushAsync();
        var csvBytes = memoryStream.ToArray();
        
        return File(csvBytes, "text/csv", "livros.csv");
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { 
            Erro = "Erro ao gerar arquivo CSV", 
            Detalhes = ex.Message 
        });
    }
}
```

## Como Testar

### 1. **Via Swagger UI:**
   - Acesse: `http://localhost:5219/swagger`
   - Procure pelo endpoint `GET /api/livros/exportar-csv`
   - Clique em "Try it out" e "Execute"
   - O arquivo CSV será baixado automaticamente

### 2. **Via Browser:**
   - Acesse diretamente: `http://localhost:5219/api/livros/exportar-csv`
   - O browser iniciará o download do arquivo `livros.csv`

### 3. **Via cURL:**
   ```bash
   curl -o livros.csv "http://localhost:5219/api/livros/exportar-csv"
   ```

### 4. **Via arquivo .http (VS Code):**
   ```http
   GET http://localhost:5219/api/livros/exportar-csv
   Accept: text/csv
   ```

## Status de Retorno

- **200 OK**: Arquivo CSV gerado com sucesso
- **500 Internal Server Error**: Erro ao gerar o arquivo CSV

O endpoint está pronto para uso e integra perfeitamente com o sistema existente!