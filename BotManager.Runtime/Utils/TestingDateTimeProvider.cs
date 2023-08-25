namespace BotManager.Runtime.Utils;

/// <summary>
/// An implementation of <see cref="IDateTimeProvider"/> that can be used for testing.
/// You can set <see cref="Now"/> manually and in a controlled way, so it don't return the real system time.
/// </summary>
public sealed class TestingDateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Gets and sets the current date time that is returned by this provider.
    /// </summary>
    public DateTime Now { get; set; }
}