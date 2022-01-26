# Words API

1. 'word counter' endpoint
    - Receives a text input and counts the number of appearances for each word in the input.
    - The endpoint should not return data (but you may return success status, etc).
    - The endpoint should be able to accept the input in 3 ways:
        - A simple string sent in the request.
        - A file path for a file in the server’s filesystem (the contents will be used as input).
        - A URL (the data returned from the URL will be used as input).
    - The input may be very large (up to tens of gigabytes).
    - The results (the number of appearances of each word) should be persisted.

2. A 'word statistics' endpoint
    - Receives a word and returns the number of times the word appeared so far.

## API docs

You can access the swagger ui generated docs on `https://localhost:7253/swagger`

## Tools and Technology

- C#
- ASP.net core 6.
- EntityFrameword InMemory 6.
- Xunit.

## Technical notes

- I used InMemory DB for presisting the data, when app starts, I seed DB with basic data for testing ("my" = 3, "what"=1).
- You can run the application through the docker file or install .net core 6.

## Architecture

- I followed the onion architecture in my application, splited the application to separate layers.

## Commands

- You can run it by download .net core(https://dotnet.microsoft.com/en-us/download/dotnet/6.0) Or run docker, there is a docker file in root folder.
- `dotnet build`: builds the project;
- `dotnet run`: starts the server in development mode;
- `dotnet test`: runs tests;

## TODO

- In a real-life scenario, integration tests must be written.
