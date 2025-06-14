FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ImageConverter.WebApi/ImageConverter.WebApi.csproj", "ImageConverter.WebApi/"]

RUN dotnet restore "ImageConverter.WebApi/ImageConverter.WebApi.csproj"

COPY . .

WORKDIR "/src/ImageConverter.WebApi"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "ImageConverter.WebApi.dll"]