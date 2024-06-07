using System.ComponentModel.DataAnnotations;
using StreamSentry.Domain.ModuleSettings;

namespace StreamSentry.Domain.Module;

public class StreamerChannelSettings
{
    [Key] public ulong ChannelId { get; set; }

    public ulong GuildId { get; set; }

    public StreamerSettings StreamerSettings { get; set; }

    public bool RemoveMessage { get; set; }
}