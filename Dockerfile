# Use the official ASP.NET 8 runtime image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 80

# Use the official ASP.NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy the project file to the container and restore dependencies
WORKDIR /src
COPY ["bkpDN.csproj", "./"]
RUN dotnet restore "./bkpDN.csproj"

# Copy the rest of the application files to the container
COPY . .

# Build the application
WORKDIR /src
RUN dotnet build "bkpDN.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "bkpDN.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bkpDN.dll"]
