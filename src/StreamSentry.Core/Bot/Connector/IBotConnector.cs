namespace StreamSentry.Core.Bot.Connector;

/// <summary>
///     Discord bot connector.
/// </summary>
public interface IBotConnector
{
    /// <summary>
    ///     Connect the bot to Discord.
    /// </summary>
    Task Connect();

    /// <summary>
    ///     Disconnect the bot from Discord.
    /// </summary>
    Task Disconnect();
}