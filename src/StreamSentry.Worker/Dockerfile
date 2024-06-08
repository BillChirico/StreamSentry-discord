# Use the official Microsoft .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY /src/StreamSentry.Worker.csproj ./
RUN dotnet restore

# Copy the rest of the project files
COPY . ./

# Build the project
RUN dotnet publish -c Release -o out

# Use the official Microsoft .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Create a user to run our app
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

# Copy the build output from the build-env stage
COPY --from=build-env /app/out ./

# Set the entry point for the Docker container
ENTRYPOINT ["dotnet", "StreamSentry.Worker.dll"]