# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["ContainerApp.csproj", "."]
RUN dotnet restore "ContainerApp.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "ContainerApp.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ContainerApp.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entry point (replace with your actual DLL name)
CMD ["dotnet", "ContainerApp.dll"]