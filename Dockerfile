# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore any dependencies
COPY src/StreamSentry.Worker/StreamSentry.Worker.csproj ./src/StreamSentry.Worker/
RUN dotnet restore ./src/StreamSentry.Worker/StreamSentry.Worker.csproj

# Copy the entire project and build it
COPY src/ ./src/
RUN dotnet publish ./src/StreamSentry.Worker/StreamSentry.Worker.csproj -c Release -o /app/out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/runtime:8.0

# Set the working directory inside the container
WORKDIR /app

# Copy the build artifacts from the build stage
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "StreamSentry.Worker.dll"]