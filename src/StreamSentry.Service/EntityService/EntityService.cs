namespace StreamSentry.Service.EntityService;

/// <summary>
///     EntityService is a concrete implementation of the EntityServiceBase class.
///     It is used to handle entity-related operations in a specific context.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class EntityService<T> : EntityServiceBase<T>
    where T : class
{
    /// <summary>
    ///     Initializes a new instance of the EntityService class.
    /// </summary>
    /// <param name="context">The Volvox.Helios context in which the service operates.</param>
    /// <param name="dispatch">A dispatcher used to trigger events when an entity is created, updated or deleted.</param>
    public EntityService(VolvoxHeliosContext context,
        EntityChangedDispatcher<T> dispatch)
        : base(context, dispatch)
    {
    }
}