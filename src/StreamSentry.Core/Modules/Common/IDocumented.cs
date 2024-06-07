using Volvox.Helios.Core.Modules.Common;

namespace StreamSentry.Core.Modules.Common;

public interface IDocumented
{
    /// <summary>
    ///     Represents the name of the documented item.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Represents the version of the documented item.
    /// </summary>
    string Version { get; }

    /// <summary>
    ///     Provides a description of the documented item.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Determines whether the documented item is configurable.
    /// </summary>
    bool Configurable { get; }

    /// <summary>
    ///     Represents the release state of the documented item.
    /// </summary>
    ReleaseState ReleaseState { get; }
}