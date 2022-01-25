namespace Words.Services;
public interface IWordsParserService
{
   Task<string> ParseInput(
        string? sentence,
        Uri? fileUrl,
        MemoryStream? fileStream);
}
