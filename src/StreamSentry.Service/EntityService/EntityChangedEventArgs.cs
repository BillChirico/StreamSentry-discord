namespace StreamSentry.Service.EntityService;

public class EntityChangedEventArgs<T>
{
    internal EntityChangedEventArgs(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}