using Microsoft.EntityFrameworkCore;
using StreamSentry.Domain.Module;
using StreamSentry.Domain.ModuleSettings;

namespace StreamSentry.Service;

public class StreamSentryContext(DbContextOptions<StreamSentryContext> options) : DbContext(options)
{
    #region Roles

    public DbSet<WhiteListedRole> WhiteListedRoles { get; set; }

    #endregion

    #region Streamer

    public DbSet<StreamerSettings> StreamerSettings { get; set; }

    public DbSet<StreamerChannelSettings> StreamerChannelSettings { get; set; }

    public DbSet<StreamAnnouncerMessage> StreamAnnouncerMessages { get; set; }

    #endregion
}