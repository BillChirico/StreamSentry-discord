# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the official .NET SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StreamSentry/StreamSentry.csproj", "StreamSentry/"]
RUN dotnet restore "StreamSentry/StreamSentry.csproj"
COPY . .
WORKDIR "/src/StreamSentry"
RUN dotnet build "StreamSentry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StreamSentry.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamSentry.dll"]