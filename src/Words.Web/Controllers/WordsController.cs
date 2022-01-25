using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Words.Services;

namespace Words.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WordsController : ControllerBase
{
    private readonly IWordsService _wordsService;
    private readonly IWordsParserService _wordsParserService;

    public WordsController(IWordsService wordsService, IWordsParserService wordsParserService)
    {
        _wordsService = wordsService;
        _wordsParserService = wordsParserService;
    }

    /// <summary>
    /// add word counter
    /// </summary>
    /// <param name="input">The word</param>
    /// <param name="fileUrl">The url</param>
    /// <param name="file">The file</param>
    /// <returns>NoContentResult</returns>
    [HttpPost]
    [DisableRequestSizeLimit, RequestFormLimits(
        MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
    public async Task<NoContentResult> Post(
        [FromBody, ModelBinder(BinderType = typeof(EmptyBodyModelBinder<WordsCounterModel>))] WordsCounterModel input,
        [FromForm] IFormFile? file)
    {
        using var fileStream = new MemoryStream();
        if (file != null && file.Length > 0) 
        {
            file.CopyTo(fileStream);
        }

        // parse the three optional inputs and return string to presist it in the db.
        var sentence = await _wordsParserService.ParseInput(input.sentence, input.url, fileStream);
        
        // presist the words in the db.
        await _wordsService.CountWords(sentence);

        return NoContent();
    }

    /// <summary>
    /// Get WordS tatistics
    /// </summary>
    /// <param name="term">The word</param>
    /// <returns>OkResult</returns>
    [HttpGet]
    public async Task<int> Get(string term)
    {
        var count = await _wordsService.GetWordStatistics(term);

        return count;
    }
}
