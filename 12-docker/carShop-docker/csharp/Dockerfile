FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UniversalCarShop.Entities/UniversalCarShop.Entities.csproj", "UniversalCarShop.Entities/"]
COPY ["UniversalCarShop.Infrastructure/UniversalCarShop.Infrastructure.csproj", "UniversalCarShop.Infrastructure/"]
COPY ["UniversalCarShop.UseCases/UniversalCarShop.UseCases.csproj", "UniversalCarShop.UseCases/"]
COPY ["UniversalCarShop.Web/UniversalCarShop.Web.csproj", "UniversalCarShop.Web/"]
COPY ["ReportServer.Client/ReportServer.Client.csproj", "ReportServer.Client/"]

RUN dotnet restore "UniversalCarShop.Web/UniversalCarShop.Web.csproj"

COPY . .

WORKDIR "/src/UniversalCarShop.Web"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "UniversalCarShop.Web.dll"]