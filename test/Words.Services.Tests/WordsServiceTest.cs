using Words.DataAccess;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace Words.Services.Tests;

public class WordsServiceTests : InMemoryDbContext
{
    private readonly WordsService _wordService;

    public WordsServiceTests()
    {
        _wordService = new WordsService(DbContext);
    }

    [Fact]
    public async Task GetWordStatistics_Should_Return_Count()
    {
        // prepare
        var words = new List<Word>
        {
            new Word
            {
                Id = 1,
                Term = "we",
                Count = 2
            },
            new Word
            {
                Id = 2,
                Term = "what",
                Count = 1
            }
        };

        await DbContext.Words.AddRangeAsync(words);
        await DbContext.SaveChangesAsync();

        // act
        var actual = await _wordService.GetWordStatistics("we");

        // assert
        actual.Should().Be(2);
    }

    [Fact]
    public async Task GetWordStatistics_ForNotAvailable_Should_ThrowException()
    {
        // act
        var actualException = await Assert.ThrowsAsync<Exception>(() => _wordService.GetWordStatistics("notExist"));

        // assert
        actualException.Message.Should().Be("couldn't find this word");
    }

    
    [Fact]
    public async Task CoundNewWords_Should_AddNewWordsToDB()
    {
        // prepare
        var sentence = "my name is (what?), my name is (who?)";

        // act
        await _wordService.CountWords(sentence);

        // assert
        var actual = await DbContext.Words.ToListAsync();
        var expected = new List<Word>
        {
            new Word
            {
                Id = 1,
                Term = "my",
                Count = 2
            },
            new Word
            {
                Id = 2,
                Term = "name",
                Count = 2
            },
            new Word
            {
                Id = 3,
                Term = "is",
                Count = 2
            },
            new Word
            {
                Id = 4,
                Term = "what",
                Count = 1
            },
            new Word
            {
                Id = 5,
                Term = "who",
                Count = 1
            }
        };
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task CoundNewWords_Should_AddExistingWordsToDB()
    {
        // prepare
        var words = new List<Word>
        {
            new Word
            {
                Id = 1,
                Term = "is",
                Count = 5
            }
        };

        await DbContext.Words.AddRangeAsync(words);
        await DbContext.SaveChangesAsync();
        var sentence = "my name is (what?)";

        // act
        await _wordService.CountWords(sentence);

        // assert
        var actual = await DbContext.Words.ToListAsync();
        var expected = new List<Word>
        {
            new Word
            {
                Id = 1,
                Term = "is",
                Count = 6
            },
            new Word
            {
                Id = 2,
                Term = "my",
                Count = 1
            },
            new Word
            {
                Id = 3,
                Term = "name",
                Count = 1
            },
            new Word
            {
                Id = 4,
                Term = "what",
                Count = 1
            }
        };
        actual.Should().BeEquivalentTo(expected);
    }
}