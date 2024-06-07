using System.ComponentModel.DataAnnotations;

namespace StreamSentry.Domain.ModuleSettings;

public class ModuleSettings
{
    [Key] public ulong GuildId { get; set; }

    public bool Enabled { get; set; }
}