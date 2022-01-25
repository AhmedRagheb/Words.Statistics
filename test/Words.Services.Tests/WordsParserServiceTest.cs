using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Net.Http;
using Moq;
using System;
using System.Threading;
using System.Net;
using Moq.Protected;
using System.IO;
using System.Text;

namespace Words.Services.Tests;

public class WordsParserServiceTest
{
    private readonly WordsParserService _wordsParserService;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

    public WordsParserServiceTest()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _wordsParserService = new WordsParserService(_mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task ParseInput_WithNullInputs_Should_ThrowException()
    {
        // act
        var actualException = await Assert.ThrowsAsync<Exception>(() => _wordsParserService.ParseInput(string.Empty, null, null));

        // assert
        actualException.Message.Should().Be("couldn't parse input");
    }

    [Fact]
    public async Task ParseInput_WithString_Should_ReturnString()
    {
        // act
        var actual = await _wordsParserService.ParseInput("we", null, null);

        // assert
        actual.Should().Be("we");
    }

    [Fact]
    public async Task ParseInput_WithUrl_Should_ReadUrlContent()
    {
        // given
        var uri = new Uri("https://myInput.nl");
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("what"),
            });
 
        var client = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);


        // act
        var actual = await _wordsParserService.ParseInput(string.Empty, uri, null);

        // assert
        actual.Should().Be("what");
    }

    [Fact]
    public async Task ParseInput_WithStream_Should_PraseItAndReturnResult()
    {
        // given
        var stream = new MemoryStream(Encoding.ASCII.GetBytes("we"));

        // act
        var actual = await _wordsParserService.ParseInput(string.Empty, null, stream);

        // assert
        actual.Should().Be("we");
    } 
}
