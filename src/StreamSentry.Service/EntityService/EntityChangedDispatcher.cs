namespace StreamSentry.Service.EntityService;

/// <summary>
///     Class that dispatches events when an entity is created, updated, or deleted.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class EntityChangedDispatcher<T>
{
    /// <summary>
    ///     Event triggered when an entity is created.
    /// </summary>
    public event EventHandler<EntityChangedEventArgs<T>> EntityCreated;

    /// <summary>
    ///     Event triggered when an entity is updated.
    /// </summary>
    public event EventHandler<EntityChangedEventArgs<T>> EntityUpdated;

    /// <summary>
    ///     Event triggered when an entity is deleted.
    /// </summary>
    public event EventHandler<EntityChangedEventArgs<T>> EntityDeleted;

    /// <summary>
    ///     Method to invoke the EntityCreated event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="entity">The created entity.</param>
    internal void OnEntityCreated(object sender, T entity)
    {
        EntityCreated?.Invoke(sender, new EntityChangedEventArgs<T>(entity));
    }

    /// <summary>
    ///     Method to invoke the EntityUpdated event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="entity">The updated entity.</param>
    internal void OnEntityUpdated(object sender, T entity)
    {
        EntityUpdated?.Invoke(sender, new EntityChangedEventArgs<T>(entity));
    }

    /// <summary>
    ///     Method to invoke the EntityDeleted event.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="entity">The deleted entity.</param>
    internal void OnEntityDeleted(object sender, T entity)
    {
        EntityDeleted?.Invoke(sender, new EntityChangedEventArgs<T>(entity));
    }
}