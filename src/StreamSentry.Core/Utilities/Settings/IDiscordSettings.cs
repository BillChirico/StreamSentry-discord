using Microsoft.Extensions.Configuration;

namespace StreamSentry.Core.Utilities.Settings;

public interface IDiscordSettings
{
    string Token { get; }

    string ClientId { get; }

    IConfiguration Config { get; }
}