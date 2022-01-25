namespace Words.Services;
public interface IWordsService
{
    Task<int> GetWordStatistics(string term);

    Task CountWords(string sentence);
}
