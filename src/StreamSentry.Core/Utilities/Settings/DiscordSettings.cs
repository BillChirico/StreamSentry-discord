using Microsoft.Extensions.Configuration;

namespace StreamSentry.Core.Utilities.Settings;

public class DiscordSettings : IDiscordSettings
{
    public DiscordSettings(IConfiguration config)
    {
        Token = config["Discord:Token"];
        ClientId = config["Discord:ClientID"];
        Config = config;
    }

    public string Token { get; }

    public string ClientId { get; }

    public IConfiguration Config { get; }
}