using StreamSentry.Domain.Module;

namespace StreamSentry.Domain.ModuleSettings;

public class StreamerSettings : ModuleSettings
{
    public List<StreamerChannelSettings> ChannelSettings { get; set; }

    public List<StreamAnnouncerMessage> StreamMessages { get; set; }

    public bool StreamerRoleEnabled { get; set; }

    public ulong RoleId { get; set; }

    public List<WhiteListedRole> WhiteListedRoleIds { get; set; }
}