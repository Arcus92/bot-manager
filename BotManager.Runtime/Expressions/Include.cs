using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Includes and executes an external json config.
/// The included file is only read and parsed once on the first execution.
/// <para>
/// Returns: Return value that is returned by the executed file.
/// </para>
/// </summary>
[JsonConverter(typeof(IncludeConverter))]
public class Include : IExpression
{
    internal const string Tag = "Include";
    
    /// <summary>
    /// Empty constructor for serialization.
    /// </summary>
    public Include()
    {
        FileName = string.Empty;
    }
    
    /// <summary>
    /// Creates an expression includes another json file and runs it.
    /// </summary>
    /// <param name="fileName">The filename of the json file to execute.</param>
    public Include(string fileName)
    {
        FileName = fileName;
    }
    
    /// <summary>
    /// Gets and sets the filename of the json file to execute.
    /// The file path can be absolute or relative to the root config file.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// The cached expression.
    /// </summary>
    private IExpression? _expression;

    /// <summary>
    /// Flag if the file has been tried to read. If the read failed, this flag is set anyway, so we won't try it over
    /// and over again.
    /// </summary>
    private bool _triedToRead;

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        // Read the config file
        if (!_triedToRead)
        {
            _expression = ReadFile(context);
            _triedToRead = true;
        }

        if (_expression is null)
            return null;

        return await context.ExecuteAsync(_expression, returnType);
    }

    /// <summary>
    /// Read the included file and returns the imported expression.
    /// </summary>
    /// <param name="context">The current runtime context.</param>
    /// <returns>
    /// Returns the expression that was read from the file.
    /// Returns <c>null</c> if any errors occured.
    /// </returns>
    private IExpression? ReadFile(RuntimeContext context)
    {
        if (string.IsNullOrEmpty(FileName))
            return null;
        
        var fileName = FileName;
        context.Logger.Info(Tag, $"Including '{fileName}'...");
        
        // Resolve relative file from config root...
        if (!Path.IsPathRooted(fileName))
        {
            if (context.RootPath is null)
            {
                context.Logger.Error(Tag, "Tried to include relative filename, but no RootPath provided! Skipping include.");
                return null;
            }
            
            fileName = Path.Combine(context.RootPath, fileName);
        }

        // Check if file exists...
        if (!File.Exists(fileName))
        {
            context.Logger.Error(Tag, $"Include file '{fileName}' was not found! Skipping include.");
            return null;
        }

        try
        {
            return IExpression.Deserialize(fileName);
        }
        catch (Exception ex)
        {
            context.Logger.Error(Tag, $"Failed to parse include file '{fileName}': {ex}");
            return null;
        }
    }
}