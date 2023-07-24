using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Logical and operator. It returns <c>true</c> if all expression in this list are <c>true</c> or if the list is empty.
/// Execution is stopped, once the first entry returns <c>false</c>.
/// <para>
/// Returns: <see cref="bool"/> value.
/// </para>
/// </summary>
[JsonConverter(typeof(ListConverter<And>))]
public sealed class And : List<IExpression?>, IExpression
{
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Count == 0)
            return true;
        
        foreach (var operation in this)
        {
            var result = await context.ExecuteAsync<bool>(operation);
            if (!result)
                return false;
        }

        return true;
    }
}