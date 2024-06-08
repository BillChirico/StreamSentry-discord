using Discord.WebSocket;
using StreamSentry.Core.Bot;
using StreamSentry.Core.Modules.Common;
using StreamSentry.Core.Utilities.Settings;
using StreamSentry.Worker;
using Volvox.Helios.Web;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddConfiguration(ConfigurationHelper.GetDefaultConfiguration());

// Worker
builder.Services.AddHostedService<StreamSentryWorker>();

// Bot
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<IBot, Bot>();

// All Modules
builder.Services.AddSingleton<IList<IModule>>(s => s.GetServices<IModule>().ToList());
//builder.Services.AddSingleton<IList<ICommand>>(s => s.GetServices<ICommand>().ToList());

// Settings
builder.Services.AddSingleton<IDiscordSettings, DiscordSettings>();

// Cache
builder.Services.AddMemoryCache();

var host = builder.Build();
await host.RunAsync();