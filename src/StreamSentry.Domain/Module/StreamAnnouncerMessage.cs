using System.ComponentModel.DataAnnotations;
using StreamSentry.Domain.ModuleSettings;

namespace StreamSentry.Domain.Module;

public class StreamAnnouncerMessage
{
    [Key] public int Id { get; set; }

    public ulong UserId { get; set; }

    public ulong MessageId { get; set; }

    public ulong ChannelId { get; set; }

    public ulong GuildId { get; set; }

    public virtual StreamerSettings StreamerSettings { get; set; }
}