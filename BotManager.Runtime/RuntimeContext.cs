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
    }
    
    /// <summary>
    /// Creates a copy of the <paramref name="original"/> runtime. Use this if you plan to run expression outside the
    /// current stack. Otherwise multiple expression can mess with the data in parallel.
    /// </summary>
    /// <param name="original"></param>
    public RuntimeContext(RuntimeContext original)
    {
        // Copy the variables
        foreach (var pair in original._variables)
        {
            _variables.Add(pair.Key, pair.Value);
        }
    }
    
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
    /// The map of all variables.
    /// </summary>
    private readonly Dictionary<string, object?> _variables = new();

    /// <summary>
    /// Gets the variable.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <returns>Returns the value.</returns>
    public object? Get(string name)
    {
        return _variables[name];
    }
    
    /// <summary>
    /// Sets the variable
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">The value to set.</param>
    public void Set(string name, object? value)
    {
        _variables[name] = value;
    }
    
    #endregion Variables
}