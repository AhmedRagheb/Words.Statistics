FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

WORKDIR /app
COPY publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Words.Web.dll"]
