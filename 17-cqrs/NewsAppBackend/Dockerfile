FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NewsAppBackend.Domain/NewsAppBackend.Domain.csproj", "NewsAppBackend.Domain/"]
COPY ["NewsAppBackend.Application/NewsAppBackend.Application.csproj", "NewsAppBackend.Application/"]
COPY ["NewsAppBackend.Infrastructure/NewsAppBackend.Infrastructure.csproj", "NewsAppBackend.Infrastructure/"]
COPY ["NewsAppBackend.WebApi/NewsAppBackend.WebApi.csproj", "NewsAppBackend.WebApi/"]

RUN dotnet restore "NewsAppBackend.WebApi/NewsAppBackend.WebApi.csproj"

COPY . .

WORKDIR "/src/NewsAppBackend.WebApi"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "NewsAppBackend.WebApi.dll"]