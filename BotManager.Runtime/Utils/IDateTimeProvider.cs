namespace BotManager.Runtime.Utils;

/// <summary>
/// An interface to wrap <c>DateTime.Now</c> in a provider for testing.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current time from this date time provider.
    /// </summary>
    DateTime Now { get; }
}