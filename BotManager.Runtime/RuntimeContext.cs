using BotManager.Runtime.Utils;

namespace BotManager.Runtime;

/// <summary>
/// The runtime context stores variables for a <see cref="IExpression"/> run.
/// Use <see cref="ExecuteAsync(BotManager.Runtime.IExpression?)"/> to run an expression.
/// </summary>
public sealed class RuntimeContext
{
    /// <summary>
    /// Creates a new empty runtime context.
    /// </summary>
    public RuntimeContext()
    {
        Locals = new();
        Functions = new();
    }
    
    /// <summary>
    /// Creates a copy of the <paramref name="original"/> runtime. Use this if you plan to run expression outside the
    /// current stack. Otherwise multiple expression can mess with the data in parallel.
    /// </summary>
    /// <param name="original">The original context to copy from.</param>
    public RuntimeContext(RuntimeContext original)
    {
        // Local variables are copied so the two runtimes don't interfere.
        Locals = new(original.Locals);
        Functions = new Storage<IExpression>(original.Functions);

        RootPath = original.RootPath;
    }

    /// <summary>
    /// Gets and sets the root path of the current config file.
    /// </summary>
    public string? RootPath { get; set; }

    /// <summary>
    /// Gets and sets the date time provider for the current runtime.
    /// </summary>
    public IDateTimeProvider DateTimeProvider { get; set; } = new DefaultDateTimeProvider();

    #region Logger
    
    /// <summary>
    /// Gets the logger.
    /// </summary>
    public Logger Logger { get; } = new();
    
    #endregion Logger

    #region Execute
    
    /// <summary>
    /// Execute the given expression but don't expect any return value.
    /// </summary>
    /// <param name="expression">The expression to run.</param>
    /// <returns>The task without any return value.</returns>
    public Task ExecuteAsync(IExpression? expression)
    {
        return ExecuteAsync(expression, null);
    }
    
    /// <summary>
    /// Execute the given operation with the given return type restriction.
    /// </summary>
    /// <param name="expression">The expression to run.</param>
    /// <param name="returnType">The type of the return value.</param>
    /// <returns></returns>
    public Task<object?> ExecuteAsync(IExpression? expression, Type? returnType)
    {
        if (expression is null)
            return Task.FromResult<object?>(null);

        return expression.ExecuteAsync(this, returnType);
    }
    
    /// <summary>
    /// Execute the given expression with the given return type restriction.
    /// </summary>
    /// <param name="expression">The expression to run.</param>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <returns>Returns the result of the expression.</returns>
    public async Task<T?> ExecuteAsync<T>(IExpression? expression)
    {
        var value = await ExecuteAsync(expression, typeof(T));

        // Handle null
        var type = typeof(T);
        if (value is null && (type.IsClass || Nullable.GetUnderlyingType(type) != null))
            return default;
        
        if (value is T typedValue)
            return typedValue;

        throw new ArgumentException("Unexpected type.");
    }
    
    #endregion Execute
    
    #region Variables

    /// <summary>
    /// The map of local variables.
    /// </summary>
    public Storage<object> Locals { get; }
    
    /// <summary>
    /// The map of local functions.
    /// </summary>
    public Storage<IExpression> Functions { get; }
    
    /// <summary>
    /// Gets a local variable by its name.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <returns>Returns the value.</returns>
    public object Get(string name)
    {
        return Locals.Get(name);
    }
    
    /// <summary>
    /// Sets a local variable.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">The value to set.</param>
    public void Set(string name, object? value)
    {
        Locals.Set(name, value);
    }
    
    #endregion Variables
}