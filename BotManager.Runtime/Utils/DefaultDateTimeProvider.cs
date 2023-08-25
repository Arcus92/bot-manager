namespace BotManager.Runtime.Utils;

/// <summary>
/// The default implementation of the <see cref="IDateTimeProvider"/> that returns the real system time.
/// </summary>
public class DefaultDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;
}