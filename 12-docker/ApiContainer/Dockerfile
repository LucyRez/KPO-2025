FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ./TestConsoleContainer/TestConsoleContainer.csproj .
RUN dotnet restore "TestConsoleContainer.csproj"

# Copy the rest of the code
COPY ./TestConsoleContainer .

# Build the application
RUN dotnet build "TestConsoleContainer.csproj" -c Release

# Publish the application
FROM build AS publish
RUN dotnet publish "TestConsoleContainer.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TestConsoleContainer.dll"]