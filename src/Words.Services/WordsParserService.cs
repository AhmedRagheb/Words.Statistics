namespace Words.Services;
public class WordsParserService : IWordsParserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WordsParserService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task<string> ParseInput(
        string? sentence,
        Uri? fileUrl,
        MemoryStream? fileStream)
    {
        var result = string.Empty;

        if (!string.IsNullOrEmpty(sentence))
        {
            result = sentence;
        }
        else if (fileUrl != null)
        {
            result = await GetUrlContent(fileUrl);
        }
        else if (fileStream != null && fileStream.Length > 0)
        {
            result = await GetFileContent(fileStream);
        }

        if (string.IsNullOrEmpty(result))
        {
            throw new Exception("couldn't parse input");
        }

        return result;
    }

    private async Task<string> GetFileContent(MemoryStream fileStream)
    {
        try
        {
            var pos = fileStream.Position;
            fileStream.Position = 0;

            var stream = new StreamReader(fileStream);
            var fileContent = await stream.ReadToEndAsync();
            fileStream.Position = pos;

            return fileContent;
        }
        catch
        {
            throw new Exception("couldn't parse file content");
        }
    }

    private async Task<string?> GetUrlContent(Uri fileUrl)
    {
        try
        {
            var result = string.Empty;
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                fileUrl);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var fileContent = await httpResponseMessage.Content.ReadAsStringAsync();

                result = fileContent;
            }

            return result;
        }
        catch
        {
            throw new Exception("couldn't get request and parse the response");
        }
    }
}
