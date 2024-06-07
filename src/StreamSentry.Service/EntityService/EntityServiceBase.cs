using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace StreamSentry.Service.EntityService;

public abstract class EntityServiceBase<T> : IEntityService<T>
    where T : class
{
    protected readonly StreamSentryContext Context;

    /// <summary>
    ///     Initialize a new EntityService class.
    /// </summary>
    /// <param name="context">Volvox.Helios context.</param>
    /// <param name="dispatch"></param>
    protected EntityServiceBase(StreamSentryContext context,
        EntityChangedDispatcher<T> dispatch)
    {
        Context = context;
        Dispatch = dispatch;
    }

    public EntityChangedDispatcher<T> Dispatch { get; }

    /// <inheritdoc />
    public virtual async Task<T> Find(params object[] keys)
    {
        return await Context.FindAsync<T>(keys);
    }

    /// <inheritdoc />
    public virtual async Task<List<T>> Get(Expression<Func<T, bool>> filter,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetIncludesQuery(includes);
        return await query.Where(filter).ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
    {
        var query = GetIncludesQuery(includes);
        return await query.ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task Create(T entity)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync();
        Dispatch.OnEntityCreated(this, entity);
    }

    ///<inheritdoc />
    public virtual async Task CreateBulk(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
        await Context.SaveChangesAsync();

        foreach (var entity in entities)
            Dispatch.OnEntityCreated(this, entity);
    }

    /// <inheritdoc />
    public virtual async Task Update(T entity)
    {
        if (!Context.Set<T>().Local.Any(e => e == entity))
        {
            throw new InvalidOperationException("You must use an attached entity when updating.");
        }

        await Context.SaveChangesAsync();
        Dispatch.OnEntityUpdated(this, entity);
    }

    /// <inheritdoc />
    public virtual async Task Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
        Dispatch.OnEntityDeleted(this, entity);
    }

    ///<inheritdoc />
    public virtual async Task RemoveBulk(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
        await Context.SaveChangesAsync();

        foreach (var entity in entities)
            Dispatch.OnEntityDeleted(this, entity);
    }

    /// <summary>
    ///     Get the database query with added includes.
    /// </summary>
    /// <param name="includes">Properties to eagerly load.</param>
    /// <returns>Query set to the type and includes added.</returns>
    protected IQueryable<T> GetIncludesQuery(Expression<Func<T, object>>[] includes)
    {
        var query = Context.Set<T>().AsQueryable();

        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}