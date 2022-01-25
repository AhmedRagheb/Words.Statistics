using Words.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Words.Services;
public class WordsService : IWordsService
{
    private readonly WordsDbContext _wordsDbContext;

    public WordsService(WordsDbContext wordsDbContext) => _wordsDbContext = wordsDbContext;

    public async Task CountWords(string sentence)
    {
        sentence = Utils.RemoveSpecialChars(sentence);

        var words = sentence.Split(' ').ToList();
        var wordGroups = GetWordsGroups(words);

        foreach (var wordGroup in wordGroups)
        {
            var existingWord = await GetWordByTerm(wordGroup.Term);

            await AddOrUpdateWord(wordGroup, existingWord);
        }

        await _wordsDbContext.SaveChangesAsync();
    }

    public async Task<int> GetWordStatistics(string term)
    {
        var word = await _wordsDbContext.Words.SingleOrDefaultAsync(word => word.Term == term);

        if (word == null)
        {
            throw new Exception("couldn't find this word");
        }

        return word.Count;
    }

    private static List<Word> GetWordsGroups(List<string> words)
    {
        return words
            .GroupBy(word => word)
            .Select(word =>
                new Word
                {
                    Term = word.Key,
                    Count = word.Count()
                })
            .ToList();
    }

    private async Task<Word?> GetWordByTerm(string term)
    {
        return await _wordsDbContext
            .Words
            .Where(word => word.Term == term)
            .SingleOrDefaultAsync();
    }

    private async Task AddOrUpdateWord(Word wordGroup, Word? existingWord)
    {
        if (existingWord != null)
        {
            existingWord.Count += wordGroup.Count;
        }
        else
        {
            await _wordsDbContext.Words.AddAsync(wordGroup);
        }
    }
}
