namespace StreamSentry.Service.ModuleSettings;

public class ModuleSettingsChangedArgs<T>
{
    internal ModuleSettingsChangedArgs(T settings)
    {
        Settings = settings;
    }

    public T Settings { get; }
}