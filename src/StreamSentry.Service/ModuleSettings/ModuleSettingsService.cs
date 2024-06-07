using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace StreamSentry.Service.ModuleSettings;

/// <inheritdoc />
public class ModuleSettingsService<T>(IServiceScopeFactory scopeFactory, IMemoryCache cache) : IModuleSettingsService<T>
    where T : Domain.ModuleSettings.ModuleSettings
{
    private readonly Dictionary<ulong, List<string>> _guildCacheKeys = new();

    public event EventHandler<ModuleSettingsChangedArgs<T>> SettingsChanged;

    /// <inheritdoc />
    public async Task SaveSettings(T settings)
    {
        // Create a new scope to get the db context.
        using var scope = scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<StreamSentryContext>();
        var guildSetting = await context.Set<T>().FirstOrDefaultAsync(s => s.GuildId == settings.GuildId);

        // Replace the setting if it already exists.
        if (guildSetting != null)
            context.Entry(guildSetting).CurrentValues.SetValues(settings);

        // Add the setting.
        else
            await context.AddAsync(settings);

        await context.SaveChangesAsync();

        // Reset all the cache keys for the specified guild.
        if (_guildCacheKeys.TryGetValue(settings.GuildId, out var key))
            foreach (var cacheKey in key)
                cache.Remove(cacheKey);

        OnSettingsChanged(settings);
    }

    /// <inheritdoc />
    public async Task<T> GetSettingsByGuild(ulong guildId, params Expression<Func<T, object>>[] includes)
    {
        // Create a new scope to get the db context.
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StreamSentryContext>();

        // Cache the settings.
        var cacheKey = GetCacheKey(guildId, includes);

        var cacheValue = await cache.Set(cacheKey, GetValue(guildId, includes, context),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        // Initialize new guild cache.
        if (!_guildCacheKeys.ContainsKey(guildId))
            _guildCacheKeys.Add(guildId, []);

        // Add cache key to the list.
        if (!_guildCacheKeys[guildId].Contains(cacheKey))
            _guildCacheKeys[guildId].Add(cacheKey);

        return (T)cacheValue.Value;
    }

    /// <inheritdoc />
    public async Task RemoveSetting(T settings)
    {
        // Create a new scope to get the db context.
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StreamSentryContext>();

        context.Remove(settings);

        await context.SaveChangesAsync();

        OnSettingsChanged(settings);
    }

    /// <summary>
    ///     Asynchronously retrieves the settings for a specific guild and includes specified navigation properties.
    /// </summary>
    /// <param name="guildId">The unique identifier of the guild for which settings are to be retrieved.</param>
    /// <param name="includes">An array of lambda expressions representing navigation properties to be included in the query.</param>
    /// <param name="context">The database context to be used for the query.</param>
    /// <returns>A tuple containing the IQueryable query and the first or default result of the query.</returns>
    private static async Task<(object Query, object Value)> GetValue(ulong guildId,
        Expression<Func<T, object>>[] includes, StreamSentryContext context)
    {
        // Create a queryable set of the generic type T.
        var query = context.Set<T>().AsQueryable();

        // If includes are provided, aggregate them into the query.
        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        // Return the query and the first or default result of the query where the guild ID matches the provided guild ID.
        return ( query, query.FirstOrDefault(s => s.GuildId == guildId) );
    }

    /// <summary>
    ///     Create a unique caching key based on the specified guild.
    /// </summary>
    /// <param name="guildId">Id of the guild.</param>
    /// <param name="includes">Navigation property includes to eager load.</param>
    /// <returns>Cache key based on the specified guild.</returns>
    private static string GetCacheKey(ulong guildId, params Expression<Func<T, object>>[] includes)
    {
        var includesKey = "";

        // Append all the includes to the cache key.
        if (includes.Length > 0)
            includesKey = includes.Aggregate(includesKey, (current, include) => current + include.Body);

        return $"Setting:{typeof(T).Name}Guild:{guildId}Includes:{includesKey}";
    }

    private void OnSettingsChanged(T settings)
    {
        SettingsChanged?.Invoke(this, new ModuleSettingsChangedArgs<T>(settings));
    }
}