namespace BotManager.Runtime.Utils;

/// <summary>
/// The default implementation of the <see cref="IDateTimeProvider"/> that returns the real system time.
/// </summary>
public sealed class DefaultDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime Now => DateTime.Now;
}