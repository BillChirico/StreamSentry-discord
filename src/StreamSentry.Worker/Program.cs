using Discord.WebSocket;
using StreamSentry.Core.Bot;
using StreamSentry.Core.Utilities.Settings;
using StreamSentry.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<StreamSentryWorker>();

// Bot
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<IBot, Bot>();

// Settings
builder.Services.AddSingleton<IDiscordSettings, DiscordSettings>();

// Cache
builder.Services.AddMemoryCache();

var host = builder.Build();
await host.RunAsync();