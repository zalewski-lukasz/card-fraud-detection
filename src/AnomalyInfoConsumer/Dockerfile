# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5261
ENV ASPNETCORE_URLS=http://+:5261
ENV ASPNETCORE_ENVIRONMENT=Development

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["AnomalyInfoConsumer.API/AnomalyInfoConsumer.API.csproj", "AnomalyInfoConsumer.API/"]
COPY ["AnomalyInfoConsumer.BackgroundServices/AnomalyInfoConsumer.BackgroundServices.csproj", "AnomalyInfoConsumer.BackgroundServices/"]
COPY ["AnomalyInfoConsumer.Models/AnomalyInfoConsumer.Models.csproj", "AnomalyInfoConsumer.Models/"]
COPY ["AnomalyInfoConsumer.Repositories/AnomalyInfoConsumer.Repositories.csproj", "AnomalyInfoConsumer.Repositories/"]
COPY ["AnomalyInfoConsumer.sln", "."]
RUN dotnet restore

# Copy the entire project files
COPY . .

# Build the project
WORKDIR /src/AnomalyInfoConsumer.API
RUN dotnet build -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnomalyInfoConsumer.API.dll"]
