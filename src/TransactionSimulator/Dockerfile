# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5162
ENV ASPNETCORE_URLS=http://+:5162
ENV ASPNETCORE_ENVIRONMENT=Development

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["TransactionSimulator.API/TransactionSimulator.API.csproj", "TransactionSimulator.API/"]
COPY ["TransactionSimulator.BackgroundWorkers/TransactionSimulator.BackgroundWorkers.csproj", "TransactionSimulator.BackgroundWorkers/"]
COPY ["TransactionSimulator.Models/TransactionSimulator.Models.csproj", "TransactionSimulator.Models/"]
COPY ["TransactionSimulator.Repositories/TransactionSimulator.Repositories.csproj", "TransactionSimulator.Repositories/"]
COPY ["TransactionSimulator.Services/TransactionSimulator.Services.csproj", "TransactionSimulator.Services/"]
COPY ["TransactionSimulator.sln", "."]
RUN dotnet restore

# Copy the entire project files
COPY . .

# Build the project
WORKDIR /src/TransactionSimulator.API
RUN dotnet build -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionSimulator.API.dll"]
