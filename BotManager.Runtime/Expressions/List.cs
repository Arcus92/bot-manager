using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// A list of <see cref="IExpression"/> that will be executed sequentially.
/// The returned values of the expressions is returns as an array if <c>returnType</c> is not set to null.
/// <para>
/// Returns: <see cref="Array"/> or <c>null</c>.
/// </para>
/// </summary>
[JsonConverter(typeof(ListConverter<List>))]
public class List : List<IExpression?>, IExpression
{
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        // Optimisation: If no return type is required we don't allocate the return array
        
        // No result required
        if (returnType is null)
        {
            foreach (var operation in this)
            {
                await context.ExecuteAsync(operation);
            }

            return null;
        }

        // TODO: Gets the nested list type
        var innerType = typeof(object);

        // Executes each item and returns an array with all results
        var result = new object?[Count];
        for (var i = 0; i < Count; i++)
        {
            result[i] = await context.ExecuteAsync(this[i], innerType);
        }
        return result;
        
    }
}