FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StreamSentry.Worker/StreamSentry.Worker.csproj", "StreamSentry.Worker/"]
RUN dotnet restore "StreamSentry.Worker/StreamSentry.Worker.csproj"
COPY . .
WORKDIR "/src/StreamSentry.Worker"
RUN dotnet build "StreamSentry.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StreamSentry.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamSentry.Worker.dll"]
