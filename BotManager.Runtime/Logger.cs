namespace BotManager.Runtime;

/// <summary>
/// A simple logger class that writers to the std/err output.
/// </summary>
public sealed class Logger
{
    /// <summary>
    /// Writes a message to the standard output.
    /// </summary>
    /// <param name="tag">A log tag.</param>
    /// <param name="message">The message.</param>
    public void Info(string tag, string message)
    {
        Console.WriteLine($"[{tag}] {message}");
    }

    /// <summary>
    /// Writes a message to the error output.
    /// </summary>
    /// <param name="tag">A log tag.</param>
    /// <param name="message">The message.</param>
    public void Error(string tag, string message)
    {
        Console.Error.WriteLine($"[{tag}] {message}");
    }
}