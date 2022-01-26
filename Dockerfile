FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY ["../src/Words.Web/Words.Web.csproj", "."]
COPY ["../src/Words.Services/Words.Services.csproj", "../Words.Services/"]
COPY ["../src/Words.Models/Words.Models.csproj", "../Words.Models/"]
COPY ["../src/Words.DataAccess/Words.DataAccess.csproj", "../Words.DataAccess/"]
RUN dotnet restore "./Words.Web.csproj"

COPY . .
WORKDIR "/src/."

RUN dotnet build "Words.Web.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Words.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Words.Web.dll"]
